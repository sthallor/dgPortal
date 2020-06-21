using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataGumboInterface.Service;
using DataGumboInterface.Models.RealTime;

namespace DataGumboInterfaceTest
{
    [TestClass]
    public class RealTimeTest
    {
        public static string[] rigNumbers = { "144", "147", "153", "156", "549", "550", "760", "771", "775", "778" };
        //public static string[] rigNumbers = { "775","778"  };
        const string variableNameToCheck = "Block Height";
        const string variableNameToCheckGenerator = "Generator {0} KW";

        [TestMethod]
        public void VerifyRecentRealTimeData()
        {
            RealTimeDataService<double> svc = new RealTimeDataService<double>();
            List<string> rigsWithoutHourRecentData = new List<string>();
            foreach (string rigNumber in rigNumbers)
            {
                if (!(svc.Get(rigNumber, variableNameToCheck, DateTime.Now.AddHours(-1), DateTime.Now).Any()))
                {
                    rigsWithoutHourRecentData.Add(rigNumber);
                }
            }

            if (rigsWithoutHourRecentData.Count() == rigNumbers.Count())
            {
                Assert.Fail("None of the rigs ({0}) has real time data from the last hour. Check data feed.", string.Join(",", rigNumbers));
            }
        }

        [TestMethod]
        public void VerifyAllRigsRecentRealTimeGeneratorData()
        {
            RealTimeDataService<double> svc = new RealTimeDataService<double>();
            List<string> rigsMissingGenDataOverPastDay = new List<string>();
            foreach (string rigNumber in rigNumbers)
            {
                bool gen1DataPresent = false;
                bool gen2DataPresent = false;
                bool gen3DataPresent = false;

                int hourCounter = 24;
                while (hourCounter > 1 && !gen1DataPresent)
                {
                    gen1DataPresent = svc.Get(rigNumber, string.Format(variableNameToCheckGenerator, 1), DateTime.Now.AddHours(0 - hourCounter), DateTime.Now.AddHours(0 - hourCounter + 1)).Any();
                    hourCounter--;
                }

                hourCounter = 24;
                while (hourCounter > 1 && !gen2DataPresent)
                {
                    gen2DataPresent = svc.Get(rigNumber, string.Format(variableNameToCheckGenerator, 2), DateTime.Now.AddHours(0 - hourCounter), DateTime.Now.AddHours(0 - hourCounter + 1)).Any();
                    hourCounter--;
                }

                hourCounter = 24;
                while (hourCounter > 1 && !gen3DataPresent)
                {
                    gen3DataPresent = svc.Get(rigNumber, string.Format(variableNameToCheckGenerator, 3), DateTime.Now.AddHours(0 - hourCounter), DateTime.Now.AddHours(0 - hourCounter + 1)).Any();
                    hourCounter--;
                }

                if (!(gen1DataPresent && gen2DataPresent && gen3DataPresent))
                {
                    rigsMissingGenDataOverPastDay.Add(rigNumber);
                }
            }

            if (rigsMissingGenDataOverPastDay.Count() == rigNumbers.Count())
            {
                Assert.Fail("None of the rigs ({0}) has real time data from the last hour. Check data feed.", string.Join(",", rigNumbers));
            }
        }

        [TestMethod]
        public void VerifyAllRigsRealTimeDataWithinOneDay()
        {
            RealTimeDataService<double> svc = new RealTimeDataService<double>();
            List<string> rigsWithoutDayRecentData = new List<string>(rigNumbers);
            DateTimeOffset now = DateTimeOffset.Now;

            foreach (string rigNumber in rigNumbers)
            {
                DateTimeOffset dtmSt = DateTime.Now.AddDays(-1);
                bool dataFound = false;
                for (int hourOffset = 24; hourOffset > 0 && !dataFound; hourOffset--)
                {
                    //try block height
                    try
                    {
                        if (svc.Get(rigNumber, variableNameToCheck, now.AddHours(0 - hourOffset), now.AddHours(0 - hourOffset + 1)).Any())
                        {
                            dataFound = true;
                        }
                    }
                    catch (Exception e)
                    {
                        dataFound = false;
                    }
                    // if that fails try generator 1 kw
                    try
                    {
                        if (svc.Get(rigNumber, string.Format(variableNameToCheckGenerator, 1), now.AddHours(0 - hourOffset), now.AddHours(0 - hourOffset + 1)).Any())
                        {
                            dataFound = true;
                        }
                    }
                    catch (Exception e)
                    {
                        dataFound = false;
                    }
                }

                if (dataFound)
                {
                    rigsWithoutDayRecentData.Remove(rigNumber);
                }
            }

            if (rigsWithoutDayRecentData.Any())
            {
                Assert.Fail("One or more of the rigs ({0}) has no real time data within the last day. Check data feed.", string.Join(",", rigsWithoutDayRecentData));
            }
        }

    }
}
