using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Service;
using DataGumboInterface.Models.RealTime;
using BlackGoldData;

namespace DataGumboBackfill
{
    public class FillDataGapsInRealTimeDataFromEDRQuery<T> : DataGumboBackfillQuery
    {
        public FindDataGapsInRealTimeDataQuery<T> FindGapsQuery { get; set; }
        public List<int> AssociatedWellIds { get; set; }
        public double Multiplier { get; set; }
        public string ExpectedUom { get; set; }

        private string GetUom(WellUOMForEDR wufe)
        {
            switch (VariableName)
            {
                case "Hole Measured Depth":
                    return wufe.HoleDepth;
                case "Bit Measured Depth":
                    return wufe.BitDepth;
                case "Hookload":
                    return wufe.HookLoad;
                case "Weight On Bit (Calculated)":
                    return wufe.HookLoad;
                case "Block Height":
                    return wufe.BlockHeight;
                case "Differential Pressure":
                    return wufe.DifferentialPressure;
                case "Top Drive Torque":
                    return wufe.RotaryTorque;
                case "Rate of Penetration (ROP)":
                    return wufe.OnBottomROP;
                case "Stand Pipe Pressure (SPP)":
                    return wufe.StandpipePressure;
                case "Rig State":
                    return string.Empty;
                default:
                    throw new Exception(string.Format("Don't know how to determine uom for {0}", VariableName));
            }
        }

        private object GetValue(WellEDR edr)
        {
            switch (VariableName)
            {
                case "Hole Measured Depth":
                    return edr.HoleDepth;
                case "Bit Measured Depth":
                    return edr.BitDepth;
                case "Hookload":
                    return edr.HookLoad;
                case "Weight On Bit (Calculated)":
                    return edr.HookLoad;
                case "Block Height":
                    return edr.BlockHeight;
                case "Differential Pressure":
                    return edr.DifferentialPressure;
                case "Top Drive Torque":
                    return edr.RotaryTorque;
                case "Rate of Penetration (ROP)":
                    return edr.OnBottomROP;
                case "Stand Pipe Pressure (SPP)":
                    return edr.StandpipePressure;
                case "Rig State":
                    return edr.RigState;
                default:
                    throw new Exception(string.Format("Don't know how to get edr value for {0}", VariableName));
            }
        }

        private T GetValueWithProperCast(WellEDR edr)
        {
            object o = GetValue(edr);
            if (o is double)
            {
                o = (double)o * Multiplier;
            }
            else if (o is decimal)
            {
                o = (decimal)o * (decimal)Multiplier;
            }
            return (T)Convert.ChangeType(o, typeof(T));
        }

        public void Init()
        { }

        //private bool DateRangesIntersect(DateTimeOffset fromDate1, DateTimeOffset toDate1, DateTimeOffset fromDate2, DateTimeOffset toDate2)
        //{
        //    return (fromDate1 >= fromDate2 && fromDate1 <= toDate2) ||
        //        (toDate1 >= fromDate2 && toDate1 <= toDate2) ||
        //        (fromDate2 >= fromDate1 && fromDate2 <= toDate1) ||
        //        (toDate2 >= fromDate1 && toDate2 <= toDate1);
        //}

        public void DetermineMultiplier(DateTimeOffset startDtm, DateTimeOffset endDtm)
        {
            // given the rig, variable name, and date range, try to figure out the well id(s) and the multiplier
            using (Blackgold_PRODEntities context = new Blackgold_PRODEntities())
            {
                //AssociatedWellIds = context.WellDrillPeriods.Where(wdp => wdp.ReportedRigName == RigNumber).Where(wdp => (wdp.StartDateTime >= startDtm && wdp.StartDateTime <= endDtm) || (wdp.EndDateTime >= startDtm && wdp.EndDateTime <= endDtm)).Select(wdp => wdp.Well_WellID).Distinct().ToList();
                AssociatedWellIds = context.WellDrillPeriods.Where(wdp => wdp.ReportedRigName == RigNumber).Where(wdp => 
                    //DateRangesIntersect(wdp.StartDateTime, wdp.EndDateTime, startDtm, endDtm)
                    (wdp.StartDateTime >= startDtm && wdp.StartDateTime <= endDtm) ||
                    (wdp.EndDateTime >= startDtm && wdp.EndDateTime <= endDtm) ||
                    (startDtm >= wdp.StartDateTime && startDtm <= wdp.EndDateTime) ||
                    (endDtm >= wdp.StartDateTime && endDtm <= wdp.EndDateTime)
                    ).Select(wdp => wdp.Well_WellID).Distinct().ToList();

                if (!AssociatedWellIds.Any())
                {
                    throw new Exception(string.Format("No wells found for rig {0} between {1} and {2}", RigNumber, Start, End));
                }

                IEnumerable<WellUOMForEDR> uomRecords = context.WellUOMForEDRs.Where(wufe => AssociatedWellIds.Contains(wufe.Well_WellID) && wufe.RowCreatedDate.Value < startDtm);
                WellUOMForEDR bestUomRecord = uomRecords.OrderBy(uom => uom.RowCreatedDate).Last();
                string edrUom = GetUom(bestUomRecord);

                if (edrUom == ExpectedUom)
                {
                    Multiplier = 1;
                }
                else if (edrUom == "kDaN" && ExpectedUom == "dAn")
                {
                    Multiplier = 0.001;
                }
                else if (edrUom == "unitless" && VariableName == "Top Drive Torque" && ExpectedUom == "kNm")
                {
                    //source is ft lbs and dest is kNm 
                    Multiplier = 0.0013558179483314;
                }
                else if (edrUom != ExpectedUom)
                {
                    throw new Exception(string.Format("Will need to add multiplier for converting from {0} to {1}", edrUom, ExpectedUom));
                }


                SendDebugMessage(string.Format("Data found for well ids {0}. The EDR UOM is {1} so will use multiplier {2} to convert to {3}.", string.Join(",",AssociatedWellIds.ToArray()), edrUom, Multiplier, ExpectedUom));
            }
        }

        public override void Execute()
        {
            Init();
            FindGapsQuery = new FindDataGapsInRealTimeDataQuery<T>();
            FindGapsQuery.VariableName = VariableName;
            FindGapsQuery.RigNumber = RigNumber;
            FindGapsQuery.GapThrehold = GapThrehold;
            FindGapsQuery.Start = Start;
            FindGapsQuery.End = End;
            FindGapsQuery.DgbfqeProcessing += HandleEvent;

            FindGapsQuery.Execute();
        }

        public void HandleEvent(DataGumboBackfillQueryEvent evt)
        {
            OnDataGumboBackfillQueryEvent(evt); // this'll handle logging

            DataGumboBackfillQueryDataGapEvent gapEvent = evt as DataGumboBackfillQueryDataGapEvent;
            if (gapEvent != null)
            {
                FillTheGap(gapEvent.GapStart, gapEvent.GapEnd);
            }
        }

        public void FillTheGap(DateTimeOffset startDtm, DateTimeOffset endDtm)
        {
            try
            {
                DetermineMultiplier(startDtm, endDtm);
            }
            catch (Exception e)
            {
                SendErrorMessage(string.Format("Encountered an error trying to backfill for {0} (rig {1}) between {2} and {3}", VariableName, RigNumber, startDtm, endDtm));
            }
            using (Blackgold_PRODEntities context = new Blackgold_PRODEntities())
            {
                context.Database.CommandTimeout = 300;
                IEnumerable<WellEDR> edrs = context.WellEDRs.Where(edr => edr.Well_WellID.HasValue && AssociatedWellIds.Contains(edr.Well_WellID.Value) && edr.MeasurementDateTime >= startDtm && edr.MeasurementDateTime <= endDtm).OrderBy(edr => edr.MeasurementDateTime);

                foreach (WellEDR edr in edrs)
                {
                    WriteData(edr.MeasurementDateTime.GetValueOrDefault(), GetValueWithProperCast(edr));
                }
            }
        }

        public void WriteData(DateTimeOffset dto, T value)
        {
            RealTimeDataService<T> svc = new RealTimeDataService<T>();
            RealTimeDataTuple<T> data = new RealTimeDataTuple<T> { Timestamp = dto, Value = value };
            OnDataGumboBackfillQueryEvent(new DataGumboBackfillQueryDataDataWrittenEvent(this, dto, value));
            try
            {
                svc.Put(RigNumber, VariableName, data);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string getRigState(double? value)
        {
            if (value == null)
            {
                return null;
            }
            switch ((int)(value.Value))
            {
                case 0:
                    return "Data Incomplete";
                    break;
                case 2:
                    return "Error In Data";
                    break;
                case 5:
                    return "Other";
                    break;
                case 20:
                    return "Tripping In";
                    break;
                case 25:
                    return "Trip In Connect";
                    break;
                case 30:
                    return "Tripping Out";
                    break;
                case 35:
                    return "Trip Out Connect";
                    break;
                case 40:
                    return "Back Reaming";
                    break;
                case 50:
                    return "Reaming";
                    break;
                case 60:
                    return "Circulating";
                    break;
                case 70:
                    return "Connecting";
                    break;
                case 80:
                    return "Slide Drilling";
                    break;
                case 90:
                    return "Rotate Drilling";
                    break;
                default:
                    return "unknown";
            }
        }
    }

}
