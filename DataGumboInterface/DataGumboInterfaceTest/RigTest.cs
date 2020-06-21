using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataGumboInterface.Models.Structured;
using DataGumboInterface.Service;

namespace DataGumboInterfaceTest
{
    [TestClass]
    public class RigTest
    {
        [TestMethod]
        public void VerifyOnlyOneOpenWapPerRig()
        {
            RigService rigSvc = new RigService();
            WellService wellSvc = new WellService();
            WellActivityPeriodService wapSvc = new WellActivityPeriodService();
            IEnumerable<Rig> allRigs = rigSvc.GetAll();
            Dictionary<string, List<WellActivityPeriod>> rigToOpenWapLookup = new Dictionary<string, List<WellActivityPeriod>>();

            foreach (Rig r in allRigs)
            {
                rigToOpenWapLookup[r.rigName] = new List<WellActivityPeriod>();

                IEnumerable<Well> wellsForRig = wellSvc.GetByParentId(r.rigId);
                foreach (Well w in wellsForRig)
                {
                    IEnumerable<WellActivityPeriod> wapsForWell = wapSvc.GetByParentId(w.wellId);
                    rigToOpenWapLookup[r.rigName].AddRange(wapsForWell.Where(wap => wap.startDateTime.HasValue && !wap.endDateTime.HasValue));
                }
            }

            string errorMessage = string.Empty;

            foreach (string rigName in rigToOpenWapLookup.Keys)
            {
                if (rigToOpenWapLookup[rigName].Count() > 1)
                {
                    errorMessage += string.Format("Rig {0} has {1} open well activity periods. ", rigName, rigToOpenWapLookup[rigName].Count());
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Assert.Fail(errorMessage);
            }

            if (rigToOpenWapLookup.All(kvp => kvp.Value.Count() == 0))
            {
                Assert.Fail("None of the rigs currently has an open well activity period");
            }
        }

        private static bool WapOverlap(WellActivityPeriod wap1, WellActivityPeriod wap2)
        {
            // allow them to overlap by four hours - take two hours off each extreme
            long startDate1 = wap1.startDateTime.GetValueOrDefault(DateTime.MinValue).AddHours(2).Ticks;
            long startDate2 = wap2.startDateTime.GetValueOrDefault(DateTime.MinValue).AddHours(2).Ticks;
            long endDate1 = wap1.endDateTime.GetValueOrDefault(DateTime.MaxValue).AddHours(-2).Ticks;
            long endDate2 = wap2.endDateTime.GetValueOrDefault(DateTime.MaxValue).AddHours(-2).Ticks;


            bool cond1 = (startDate1 <= startDate2 && endDate1 <= endDate2 && startDate2 >= startDate1 && startDate2 < endDate1);
            bool cond2 = (startDate1 >= startDate2 && endDate1 < endDate2);
            bool cond3 = (startDate2 >= startDate1 && endDate2 < endDate1);
            bool cond4 = (startDate2 <= startDate1 && endDate2 <= endDate1 && startDate1 >= startDate2 && startDate1 < endDate2);

            return  cond1 || cond2 || cond3 || cond4;
        }
        
        private static bool WapHasOverlaps(WellActivityPeriod wap, IEnumerable<WellActivityPeriod> waps)
        {
            return waps.Any(w => wap.wellActivityPeriodId != w.wellActivityPeriodId && WapOverlap(wap, w));
        }

        [TestMethod]
        public void VerifyNoOverlappingWaps()
        {
            RigService rigSvc = new RigService();
            WellService wellSvc = new WellService();
            WellActivityPeriodService wapSvc = new WellActivityPeriodService();
            IEnumerable<Rig> allRigs = rigSvc.GetAll();
            Dictionary<string, List<WellActivityPeriod>> rigToOverlappingWapLookup = new Dictionary<string, List<WellActivityPeriod>>();

            string[] validRigNames = { "140", "144", "156", "550", "778" };
            foreach (Rig r in allRigs.Where(r => validRigNames.Contains(r.rigName)))
            {
                List<WellActivityPeriod> allWapsForRig = new List<WellActivityPeriod>();

                IEnumerable<Well> wellsForRig = wellSvc.GetByParentId(r.rigId);
                foreach (Well w in wellsForRig)
                {
                    IEnumerable<WellActivityPeriod> wapsForWell = wapSvc.GetByParentId(w.wellId);
                    allWapsForRig.AddRange(wapsForWell);
                }

                rigToOverlappingWapLookup[r.rigName] = new List<WellActivityPeriod>();
                rigToOverlappingWapLookup[r.rigName].AddRange(allWapsForRig.Where(wap => WapHasOverlaps(wap, allWapsForRig)));
            }

            string errorMessage = string.Empty;

            foreach (string rigName in rigToOverlappingWapLookup.Keys)
            {
                if (rigToOverlappingWapLookup[rigName].Count() > 1)
                {
                    errorMessage += string.Format("Rig {0} has {1} overlapping well activity periods ({2}). ", rigName, rigToOverlappingWapLookup[rigName].Count(), string.Join(", ", rigToOverlappingWapLookup[rigName].Select(wap => string.Format("Well {0} from {1} to {2}", wap.wellId, wap.startDateTime, wap.endDateTime))));
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Assert.Fail(errorMessage);
            }
        }

    }
}
