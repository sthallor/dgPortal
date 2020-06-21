using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgnitionDb;
using System.Configuration;
using log4net;
using log4net.Core;
using System.Threading;
using System.IO;

namespace DataGumboBackfill
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static StreamWriter DataWrittenLogWriter { get; set; }

        public static int ThreadCount { get; set; }
        static void Main(string[] args)
        {
            DataWrittenLogWriter = new StreamWriter(File.Create(string.Format("BackfillValuesWritten_{0}.csv", DateTime.Now.Ticks)));
            DataWrittenLogWriter.AutoFlush = true;
            DataWrittenLogWriter.WriteLine("Rig,Variable,Timestamp,Value");

            IEnumerable<string> variableNames = new[] { 
                "Rig State",
                "Bit Measured Depth",
                "Block Height",
                "Differential Pressure",
                "Differential Pressure Set Point",
                "Differential Pressure Set Point Active",
                "Generator 1 KW",
                "Generator 2 KW",
                "Generator 3 KW",
                "Generator 4 KW",
                "Hole Measured Depth",
                "Hookload",
                "Pump 1 SPM",
                "Pump 2 SPM",
                "Pump 3 SPM",
                "Left Degrees",
                "Left Revolutions",
                "Left RPM",
                "Left Torque",
                "Right Degrees",
                "Right Revolutions",
                "Right RPM",
                "Right Torque",
                "Rate of Penetration (ROP)",
                "ROP Set Point",
                "ROP Set Point Active",
                "Stand Pipe Pressure (SPP)",
                "Top Drive Torque",
                "Top Drive Torque Set Point",
                "Top Drive Torque Set Point Active",
                "Weight On Bit (Calculated)",
                "Weight On Bit Set Point",
                "Weight On Bit Set Point Active" 
            };
            variableNames = new[] { "Rig State", "Hole Measured Depth", "Top Drive Torque", "Weight On Bit (Calculated)", "Differential Pressure", "Rate of Penetration (ROP)" };
            //variableNames = new[] { "Rig State", "Hole Measured Depth", "Weight On Bit (Calculated)", "Differential Pressure", "Rate of Penetration (ROP)" };
            //variableNames = new[] { "Hole Measured Depth" };

            var badTagNames = variableNames.Where(v => !FillDataGapsInRealTimeDataQuery<double>.VariableToTagLookup.ContainsKey(v)).ToList();

            ThreadPool.SetMinThreads(1, 0);
            ThreadPool.SetMaxThreads(3, 0);

            foreach (string variableName in variableNames)
            {
                if (variableName == "Rig State")
                {
                    //FindDataGapsInRealTimeDataQuery<string> q = new FindDataGapsInRealTimeDataQuery<string>();
                    //FillDataGapsInRealTimeDataQuery<string> q = new FillDataGapsInRealTimeDataQuery<string>();
                    FillDataGapsInRealTimeDataFromEDRQuery<string> q = new FillDataGapsInRealTimeDataFromEDRQuery<string>();
                    q.RigNumber = "785";
                    q.ExpectedUom = string.Empty;
                    q.VariableName = variableName;
                    q.GapThrehold = new TimeSpan(0, 5, 10);
                    q.Start = new DateTimeOffset(new DateTime(2019, 12, 21));
                    q.End = new DateTimeOffset(DateTime.Now.AddHours(-3));
                    q.DgbfqeProcessing += HandleEvent;
                    ThreadCount++;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoAWorkItem), q);
                }
                else if (variableName.EndsWith("Active"))
                {
                    //FindDataGapsInRealTimeDataQuery<int> q = new FindDataGapsInRealTimeDataQuery<int>();
                    //FillDataGapsInRealTimeDataQuery<bool> q = new FillDataGapsInRealTimeDataQuery<bool>();
                    FillDataGapsInRealTimeDataFromEDRQuery<bool> q = new FillDataGapsInRealTimeDataFromEDRQuery<bool>();
                    q.ExpectedUom = null;
                    q.RigNumber = "785";
                    q.VariableName = variableName;
                    q.GapThrehold = new TimeSpan(0, 5, 10);
                    q.Start = new DateTimeOffset(new DateTime(2019, 12, 22));
                    q.End = new DateTimeOffset(DateTime.Now.AddHours(-3));
                    q.DgbfqeProcessing += HandleEvent;
                    ThreadCount++;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoAWorkItem), q);
                } 
                else
                {
                    //FindDataGapsInRealTimeDataQuery<double> q = new FindDataGapsInRealTimeDataQuery<double>();
                    //FillDataGapsInRealTimeDataQuery<double> q = new FillDataGapsInRealTimeDataQuery<double>();
                    FillDataGapsInRealTimeDataFromEDRQuery<double> q = new FillDataGapsInRealTimeDataFromEDRQuery<double>();
                    q.ExpectedUom = GetExpectedUom(variableName);
                    q.RigNumber = "785";
                    q.VariableName = variableName;
                    q.GapThrehold = new TimeSpan(0, 5, 10);
                    q.Start = new DateTimeOffset(new DateTime(2019, 12, 21));
                    q.End = new DateTimeOffset(DateTime.Now.AddHours(-3));
                    q.DgbfqeProcessing += HandleEvent;
                    ThreadCount++;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoAWorkItem), q);
                }

            }
            //FindDataGapsInRealTimeDataQuery<double> q = new FindDataGapsInRealTimeDataQuery<double>();
            //q.RigNumber = "144";
            //q.VariableName = "Block Height";
            //q.GapThrehold = new TimeSpan(0, 5, 10);
            //q.Start = new DateTimeOffset(new DateTime(2019, 10, 12));
            //q.End = new DateTimeOffset(new DateTime(2019, 10, 17));
            ////q.DgbfqeProcessing += PrintEventMessage;
            //q.DgbfqeProcessing += LogEventMessage;
            //q.Execute();

            while (ThreadCount > 0)
            {
                Thread.Sleep(5000);
            }

            DataWrittenLogWriter.Dispose();
        }

        private static string GetExpectedUom(string variableName)
        {
            switch (variableName)
            {
                case "Hole Measured Depth":
                    return "meters";
                case "Bit Measured Depth":
                    return "meters";
                case "Hookload":
                    return "dAn";
                case "Weight On Bit (Calculated)":
                    return "dAn";
                case "Block Height":
                    return "m";
                case "Differential Pressure":
                    return "kPa";
                case "Top Drive Torque":
                    return "kNm";
                case "Rate of Penetration (ROP)":
                    return "m/hr";
                case "Stand Pipe Pressure (SPP)":
                    return "kpa";
                case "Rig State":
                    return string.Empty;
                default:
                    throw new Exception(string.Format("Don't know expected uom for {0}", variableName));
            }
        }

        public static void PrintEventMessage(DataGumboBackfillQueryEvent evt)
        {
            Console.WriteLine("Rig {0} Variable {1}: {2}", evt.Query.RigNumber, evt.Query.VariableName, evt.Message);
        }

        public static void HandleEvent(DataGumboBackfillQueryEvent evt)
        {
            string message = String.Format("Rig {0} Variable {1}: {2}", evt.Query.RigNumber, evt.Query.VariableName, evt.Message);
            if (evt.LogLevel == Level.Fatal)
            {
                log.Fatal(message);
            }
            else if (evt.LogLevel == Level.Error)
            {
                log.Error(message);
            }
            else if (evt.LogLevel == Level.Warn)
            {
                log.Warn(message);
            }
            else 
            {
                log.Debug(message);
            }
            DataGumboBackfillQueryDataDataWrittenEvent dataWrittenEvent = evt as DataGumboBackfillQueryDataDataWrittenEvent;

            if (dataWrittenEvent != null)
            {
                DataWrittenLogWriter.WriteLine(string.Format("{0},{1},{2},{3}", dataWrittenEvent.Query.RigNumber, dataWrittenEvent.Query.VariableName, dataWrittenEvent.Timestamp, dataWrittenEvent.Value));
            }
        }

        public static void DoAWorkItem(object o)
        {
            RunAQuery((DataGumboBackfillQuery)o);
        }

        public static void RunAQuery(DataGumboBackfillQuery q) 
        {
            q.Execute();
            ThreadCount--;
        }
    }
}
