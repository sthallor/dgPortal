using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataGumboInterface.Models.Structured;
using DataGumboInterface.Service;

namespace DataGumboInterfaceTest
{
    [TestClass]
    public class WellTest
    {
        [TestMethod]
        public void VerifyLatLong()
        {
            // This test ensures that all of the wells in the system have a lat long value set
            WellService wSvc = new WellService();
            RigService rSvc = new RigService();
            IEnumerable<Well> allWells = wSvc.GetAll();

            // ignore rig 771 - they never seem to report lat long
            Rig rig771 = rSvc.GetAll().First(r => r.rigName == "771");

            IEnumerable<Well> wellsWithBlankLatLong = allWells.Where(w => w.currentRigId != rig771.rigId && w.latitude == 0.0 && w.longitude == 0.0);

            if (wellsWithBlankLatLong.Any()) 
            {
                Assert.Fail("{0} of {1} Wells found in DataGumbo with blank lat/long: {2}", wellsWithBlankLatLong.Count(), allWells.Count(), string.Join(",", wellsWithBlankLatLong.Select(w => w.wellId.ToString())));
            }
        }

        [TestMethod]
        public void VerifyRecentTourSheet()
        {
            // This test ensures that we have processed a new operational remark at some point in the last 24 hours
            WellService wSvc = new WellService();
            List<Well> allWells = wSvc.GetAll().OrderByDescending(w => w.wellId).ToList();
            bool testPassed = false;
            int maxWellsToCheck = 30;
            int wellsChecked = 0;

            TourSheetService tsSvc = new TourSheetService();

            while (!testPassed && (wellsChecked < maxWellsToCheck))
            {
                Well w = allWells[wellsChecked];

                IEnumerable<TourSheet> tourSheetsForWell = tsSvc.GetByParentId(w.wellId);

                if (tourSheetsForWell.Any(ts => ts.rowChangedDate.HasValue))
                {
                    DateTime latestTourSheetDate = tourSheetsForWell.Where(ts => ts.rowChangedDate.HasValue).Max(ts => ts.rowChangedDate.Value);
                    TimeSpan diff = DateTime.Now - latestTourSheetDate;

                    if (diff.TotalHours < 24)
                    {
                        testPassed = true;
                    }
                }
                wellsChecked++;
            }

            if (!testPassed)
            {
                Assert.Fail("No new tour sheets in Data Gumbo in last 24 hours.  Verify that data feed is running.");
            }
        }
    }
}
