using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataGumboInterface.Models.Structured;
using DataGumboInterface.Service;
using BGWell = BlackGoldData.Well;
using BGWellDrillPeriod = BlackGoldData.WellDrillPeriod;
using BGWellConnectTime = BlackGoldData.WellConnectTime;
using BGTrippingSpeed = BlackGoldData.WellTrippingSpeed;
using BGWellDrillPeriodRemark = BlackGoldData.WellDrillPeriodRemark;
using BGWellDrillBitPeriod = BlackGoldData.WellDrillBitPeriod;

namespace DataGumboInterfaceTest
{
    [TestClass]
    public class DatabaseComparisonTest
    {
        private static BlackGoldData.Blackgold_PRODEntities EntityDataModel { get; set; }

        static DatabaseComparisonTest()
        {
            EntityDataModel = new BlackGoldData.Blackgold_PRODEntities();
        }

        private static IEnumerable<Well> GetValidWellsForTest()
        {
            RigService rSvc = new RigService();
            WellService wSvc = new WellService();

            List<Well> ret = new List<Well>();

            ret.AddRange(wSvc.GetByParentId(rSvc.GetByRigName("140").rigId).Where(w => w.spudDate.HasValue));
            ret.AddRange(wSvc.GetByParentId(rSvc.GetByRigName("144").rigId).Where(w => w.spudDate.HasValue));
            ret.AddRange(wSvc.GetByParentId(rSvc.GetByRigName("156").rigId).Where(w => w.spudDate.HasValue));
            ret.AddRange(wSvc.GetByParentId(rSvc.GetByRigName("550").rigId).Where(w => w.spudDate.HasValue));
            ret.AddRange(wSvc.GetByParentId(rSvc.GetByRigName("778").rigId).Where(w => w.spudDate.HasValue));

            return ret;
        }

        private static DateTime ConvertToDateTime(DateTimeOffset dtmo)
        {
            return new DateTime(dtmo.Year, dtmo.Month, dtmo.Day, dtmo.Hour, dtmo.Minute, dtmo.Second).ToUniversalTime();
        }

        private static DateTime ConvertToDateTime(DateTime dtm)
        {
            return new DateTime(dtm.Year, dtm.Month, dtm.Day, dtm.Hour, dtm.Minute, dtm.Second).ToUniversalTime();
        }

        private static DateTime ConvertToDateTime(DateTimeOffset? dtmo)
        {
            if (dtmo == null)
            {
                throw new NullReferenceException();
            }
            return ConvertToDateTime(dtmo.Value);
        }

        private static DateTime ConvertToDateTime(DateTime? dtm)
        {
            if (dtm == null)
            {
                throw new NullReferenceException();
            }
            return ConvertToDateTime(dtm.Value);
        }

        private static bool DateTimesDifferAsExpected(DateTime dtm1, DateTime dtm2)
        {
            TimeSpan ts = dtm1 - dtm2;
            double hoursDifference = Math.Abs(ts.TotalHours);
            return hoursDifference >= 5 && hoursDifference <= 9;
        }

        private static IEnumerable<Well> GetSomeTestWells(int howMany)
        {
            List<Well> allWells = GetValidWellsForTest().ToList();
            List<Well> ret = new List<Well>();

            Random rnd = new Random();
            for (int i = 0; i < howMany; i++)
            {
                ret.Add(allWells[rnd.Next(allWells.Count())]);
            }

//            ret.Clear();
//            ret.Add(allWells.FirstOrDefault(w => w.wellId == 664513));
            return ret;
        }

        [TestMethod]
        public void VerifyDGSpudAndRRAreInGMT()
        {
            foreach (Well dgW in GetSomeTestWells(300))
            {
                BGWell bgW = EntityDataModel.Wells.FirstOrDefault(w => w.WellID == dgW.wellId);

                DateTime dgSpud = ConvertToDateTime(dgW.spudDate);
                DateTime bgSpud = ConvertToDateTime(bgW.SpudDate);

                if (!DateTimesDifferAsExpected(dgSpud, bgSpud))
                {
                    Assert.Fail("For well {0} the black gold spud date is {1} and the data gumbo spud date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", dgW.wellId, bgSpud, dgSpud);
                }

                if (dgW.rigReleaseDate.HasValue && bgW.RigReleaseDate.HasValue)
                {
                    DateTime dgRr = ConvertToDateTime(dgW.rigReleaseDate);
                    DateTime bgRr = ConvertToDateTime(bgW.RigReleaseDate);

                    if (!DateTimesDifferAsExpected(dgRr, bgRr))
                    {
                        Assert.Fail("For well {0} the black gold rr date is {1} and the data gumbo rr date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", dgW.wellId, bgRr, dgRr);
                    }
                }
            }
        }

        [TestMethod]
        public void VerifyWellActivityPeriodStartDatesAreInGMT()
        {
            WellActivityPeriodService wapSvc = new WellActivityPeriodService();

            foreach (Well dgW in GetSomeTestWells(5))
            {
                foreach (WellActivityPeriod wap in wapSvc.GetByParentId(dgW.wellId))
                {
                    // compare the spud and rr dates between DG and BG. Should be between 5 and 9 hours difference
                    BGWellDrillPeriod bgWdp = EntityDataModel.WellDrillPeriods.FirstOrDefault(w => w.WellDrillPeriodID == wap.wellActivityPeriodId);

                    DateTime dgSt = ConvertToDateTime(wap.startDateTime);
                    DateTime bgSt = ConvertToDateTime(bgWdp.WellDrillPeriodRemarks.Min(r => r.EffectiveDate));

                    if (!DateTimesDifferAsExpected(dgSt, bgSt))
                    {
                        Assert.Fail("For wap {0} the black gold start date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", wap.wellActivityPeriodId, bgSt, dgSt);
                    }
                }
            }
        }

        [TestMethod]
        public void VerifyConnectTimeDatesAreInGMT()
        {
            ConnectTimeService ctSvc = new ConnectTimeService();

            foreach (Well dgW in GetSomeTestWells(300))
            {
                // do 10% of the records
                foreach (ConnectTime ct in ctSvc.GetByParentId(dgW.wellId).Where(c => c.connectTimeId % 10 == 0))
                {
                    // compare the spud and rr dates between DG and BG. Should be between 5 and 9 hours difference
                    BGWellConnectTime bgWct = EntityDataModel.WellConnectTimes.FirstOrDefault(w => w.WellConnectTimeID == ct.connectTimeId);

                    if (bgWct != null)
                    {
                        DateTime dgSt = ConvertToDateTime(ct.startDateTime);
                        DateTime bgSt = ConvertToDateTime(bgWct.FromDateTime);

                        if (!DateTimesDifferAsExpected(dgSt, bgSt))
                        {
                            Assert.Fail("For well connect time {0} the black gold start date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", ct.connectTimeId, bgSt, dgSt);
                        }

                        DateTime dgEn = ConvertToDateTime(ct.endDateTime);
                        DateTime bgEn = ConvertToDateTime(bgWct.ToDateTime);

                        if (!DateTimesDifferAsExpected(dgEn, bgEn))
                        {
                            Assert.Fail("For well connect time {0} the black gold end date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", ct.connectTimeId, bgEn, dgEn);
                        }
                    }
                }
            }
        }
        
        [TestMethod]
        public void VerifyTrippingSpeedTimeDatesAreInGMT()
        {
            TrippingSpeedService tsSvc = new TrippingSpeedService();

            foreach (Well dgW in GetSomeTestWells(300))
            {
                // do 10% of the records
                foreach (TrippingSpeed ts in tsSvc.GetByParentId(dgW.wellId).Where(t => t.trippingSpeedId % 10 == 0))
                {
                    // compare the spud and rr dates between DG and BG. Should be between 5 and 9 hours difference
                    BGTrippingSpeed bgTs = EntityDataModel.WellTrippingSpeeds.FirstOrDefault(w => w.WellTrippingSpeedID == ts.trippingSpeedId);

                    if (bgTs != null)
                    {
                        DateTime dgSt = ConvertToDateTime(ts.startDateTime);
                        DateTime bgSt = ConvertToDateTime(bgTs.FromDateTime);

                        if (!DateTimesDifferAsExpected(dgSt, bgSt))
                        {
                            Assert.Fail("For well tripping speed {0} the black gold start date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", ts.trippingSpeedId, bgSt, dgSt);
                        }

                        DateTime dgEn = ConvertToDateTime(ts.endDateTime);
                        DateTime bgEn = ConvertToDateTime(bgTs.ToDateTime);

                        if (!DateTimesDifferAsExpected(dgEn, bgEn))
                        {
                            Assert.Fail("For well tripping speed {0} the black gold end date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", ts.trippingSpeedId, bgEn, dgEn);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void VerifyTourSheetDatesAreInGMT()
        {
            TourSheetService tsSvc = new TourSheetService();

            foreach (Well dgW in GetSomeTestWells(5))
            {
                // do 10% of the records
                foreach (TourSheet ts in tsSvc.GetByParentId(dgW.wellId).Where(t => t.tourSheetId % 10 == 0))
                {
                    // compare the spud and rr dates between DG and BG. Should be between 5 and 9 hours difference
                    BGWellDrillPeriod bgTs = EntityDataModel.WellDrillPeriods.FirstOrDefault(w => w.WellDrillPeriodID == ts.tourSheetId);

                    DateTime dgSt = ConvertToDateTime(ts.startDateTime);
                    DateTime bgSt = ConvertToDateTime(bgTs.StartDateTime);

                    if (!DateTimesDifferAsExpected(dgSt, bgSt))
                    {
                        Assert.Fail("For tour sheet {0} the black gold start date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", ts.tourSheetId, bgSt, dgSt);
                    }

                    DateTime dgEn = ConvertToDateTime(ts.endDateTime);
                    DateTime bgEn = ConvertToDateTime(bgTs.EndDateTime);

                    if (!DateTimesDifferAsExpected(dgEn, bgEn))
                    {
                        Assert.Fail("For tour sheet {0} the black gold end date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", ts.tourSheetId, bgEn, dgEn);
                    }
                }
            }
        }

        [TestMethod]
        public void VerifyOperationalRemarkDatesAreInGMT()
        {
            OperationalRemarkService orSvc = new OperationalRemarkService();
            TourSheetService tsSvc = new TourSheetService();

            foreach (Well dgW in GetSomeTestWells(5))
            {
                // do 10% of the records
                List<OperationalRemark> opRemarksForWell = new List<OperationalRemark>();
                tsSvc.GetByParentId(dgW.wellId).ToList().ForEach(ts => opRemarksForWell.AddRange(orSvc.GetByParentId(ts.tourSheetId)));

                foreach (OperationalRemark or in opRemarksForWell.Where(o => o.operationalRemarkId % 10 == 0))
                {
                    // compare the spud and rr dates between DG and BG. Should be between 5 and 9 hours difference
                    BGWellDrillPeriodRemark bgWdpr = EntityDataModel.WellDrillPeriodRemarks.FirstOrDefault(w => w.WellDrillPeriodRemarkID == or.operationalRemarkId);

                    DateTime dgSt = ConvertToDateTime(or.startDateTime);
                    DateTime bgSt = ConvertToDateTime(bgWdpr.EffectiveDate);

                    if (!DateTimesDifferAsExpected(dgSt, bgSt))
                    {
                        Assert.Fail("For operational remark {0} the black gold start date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", or.operationalRemarkId, bgSt, dgSt);
                    }

                    DateTime dgEn = ConvertToDateTime(or.endDateTime);
                    DateTime bgEn = ConvertToDateTime(bgWdpr.ExpiryDate);

                    if (!DateTimesDifferAsExpected(dgEn, bgEn))
                    {
                        Assert.Fail("For operational remark {0} the black gold end date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", or.operationalRemarkId, bgEn, dgEn);
                    }
                }
            }
        }

        [TestMethod]
        public void VerifyFormationDatesAreInGMT()
        {
            TourSheetFormationService tsfSvc = new TourSheetFormationService();
            TourSheetService tsSvc = new TourSheetService();

            foreach (Well dgW in GetSomeTestWells(5))
            {
                // do 10% of the records
                List<TourSheetFormation> formationsForWell = new List<TourSheetFormation>();
                tsSvc.GetByParentId(dgW.wellId).ToList().ForEach(ts => formationsForWell.AddRange(tsfSvc.GetByParentId(ts.tourSheetId)));

                foreach (TourSheetFormation tsf in formationsForWell.Where(o => o.tourSheetFormationId % 10 == 0))
                {
                    // compare the spud and rr dates between DG and BG. Should be between 5 and 9 hours difference
                    BGWellDrillBitPeriod bgWdbp = EntityDataModel.WellDrillBitPeriods.FirstOrDefault(w => w.WellDrillBitPeriodID == tsf.tourSheetFormationId);

                    DateTime dgSt = ConvertToDateTime(tsf.startDateTime);
                    DateTime bgSt = ConvertToDateTime(bgWdbp.WellDrillPeriod.StartDateTime);

                    if (!DateTimesDifferAsExpected(dgSt, bgSt))
                    {
                        Assert.Fail("For formation {0} the black gold start date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", tsf.tourSheetFormationId, bgSt, dgSt);
                    }

                    DateTime dgEn = ConvertToDateTime(tsf.endDateTime);
                    DateTime bgEn = ConvertToDateTime(bgWdbp.WellDrillPeriod.EndDateTime);

                    if (!DateTimesDifferAsExpected(dgEn, bgEn))
                    {
                        Assert.Fail("For formation {0} the black gold end date is {1} and the data gumbo start date is {2}.  Expecting a 5 to 9 hour difference since black gold uses well local time and data gumbo uses GMT.", tsf.tourSheetFormationId, bgEn, dgEn);
                    }
                }
            }
        }

    }
}
