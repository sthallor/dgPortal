using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IgnitionDb;
using System.Configuration;
using System.Text;
using DataGumboInterface.Service;
using DataGumboInterface.Models.RealTime;

namespace DataGumboInterfaceTest
{
    [TestClass]
    public class IgnitionInterfaceTest
    {
        [TestMethod]
        public void CanConnect()
        {
            Util util = new Util(ConfigurationManager.AppSettings["IgnitionMySqlConnString"]);
            int rowsToGet = 1000;
            IEnumerable<GumboLogRow> recentRows = util.GetRecentRows(rowsToGet, DateTime.Now.AddDays(-1), DateTime.Now);
            Assert.AreEqual(rowsToGet, recentRows.Count());
            IEnumerable<string> tagNames = recentRows.Select(rr => rr.tagName).Distinct().ToArray();
            Assert.IsTrue(tagNames.Contains("Differential Pressure") || tagNames.Contains("Top Drive Torque") || tagNames.Contains("Standpipe Pressure"));
        }

        [TestMethod]
        public void ValuesGotToGumbo()
        {
            //DateTime startDtm = DateTime.Now.AddDays(-1).AddMinutes(20);
            DateTime startDtm = new DateTime(2019, 5, 17, 7, 0, 0);
            DateTime endDtm = startDtm.AddMinutes(10);

            RealTimeDataService<double> numericRtSvc = new RealTimeDataService<double>();
            RealTimeDataService<bool> boolRtSvc = new RealTimeDataService<bool>();
            RealTimeDataService<string> strRtSvc = new RealTimeDataService<string>();
            Util util = new Util(ConfigurationManager.AppSettings["IgnitionMySqlConnString"]);

            IEnumerable<GumboLogRow> gumboLogRows = util.GetRecentRows(1000000, startDtm, endDtm);

            gumboLogRows = gumboLogRows.Where(glr => glr.numericValue == 19905.2851562);

            StringBuilder passed = new StringBuilder();
            StringBuilder failed = new StringBuilder();
            int failedCount = 0;
            Dictionary<string, int> failedCountLookup = new Dictionary<string, int>();
            int passedCount = 0;
            Dictionary<string, int> passedCountLookup = new Dictionary<string, int>();

//            int[][] ranges = new int[][] { new[] { -1, 1, 0 }, new[] { -5, 5, 0 }, new[] { -1000, 1000, 0 }, new[] { -5000, 5000, 0 }, new[] { -30000, 180000, 0 }, new[] { -60000, 30000, 0 } };
            int[][] ranges = new int[][] { new[] { -1, 1, 0 }, new[] { -1, 5000, 0 }, new[] { -1, 15000, 0 }, new[] { -1, 30000, 0 }, new[] { -1, 45000, 0 }, new[] { -1, 60000, 0 } };

            foreach (GumboLogRow glr in gumboLogRows.Where(g => g.numericValue.HasValue))
            {
                int rIdx = 0;
                bool foundIt = false;
                IEnumerable<RealTimeDataTuple<double>> dgValues = null;
                while (!foundIt && rIdx < ranges.Length)
                {
                    if (glr.numericValue.HasValue)
                    {
                        DateTime stForRange = glr.logDtm.AddMilliseconds(ranges[rIdx][0]);
                        DateTime enForRange = glr.logDtm.AddMilliseconds(ranges[rIdx][1]);

                        dgValues = numericRtSvc.Get(glr.rig, glr.tagName, stForRange, enForRange);

                        foundIt = dgValues.Any(rtdt => rtdt.Value == glr.numericValue);
                        ranges[rIdx][2] += foundIt ? 1 : 0;
                    }
                    rIdx++;
                }
                DateTime st = glr.logDtm.AddMilliseconds(ranges.Select(r => r[0]).Min());
                DateTime en = glr.logDtm.AddMilliseconds(ranges.Select(r => r[1]).Max());
                if (!foundIt)
                {
                    if (!failedCountLookup.ContainsKey(glr.rig))
                    {
                        failedCountLookup[glr.rig] = 1;
                    }
                    else
                    {
                        failedCountLookup[glr.rig]++;
                    }
                    failed.AppendLine(string.Format("Was expecting to find {5} {0} (at {6}) in data gumbo between {1} and {2} for rig {3} but value was not found.  Values were: {4}", glr.numericValue, st, en, glr.rig, string.Join(", ", dgValues.Select(dg => string.Format("({0}: {1})", dg.Timestamp, dg.Value)).ToArray()), glr.tagName, glr.logDtm));
                    if (++failedCount >= 500000)
                    {
                        Assert.Fail(failed.ToString());
                    }
                }
                else
                {
                    passed.AppendFormat(string.Format("Found value {0} in data gumbo between {1} and {2} for rig {3}.", glr.numericValue, st, en, glr.rig));
                    if (!passedCountLookup.ContainsKey(glr.rig))
                    {
                        passedCountLookup[glr.rig] = 1;
                    }
                    else
                    {
                        passedCountLookup[glr.rig]++;
                    }
                    passedCount++;
                }
            }

            if (failed.Length > 0) 
            {
                Assert.Fail(failed.ToString());
            }
        }
    }
}
