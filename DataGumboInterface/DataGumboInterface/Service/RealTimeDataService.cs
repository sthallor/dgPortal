using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Helpers;
using DataGumboInterface.Models.RealTime;

namespace DataGumboInterface.Service
{
    public class RealTimeDataService<T>
    {
        static RealTimeDataService()
        {
            ServerName = ConfigurationManager.AppSettings["GumboRealTimeServerName"];
            ApiKey = ConfigurationManager.AppSettings["GumboRealTimeApiKey"];
            AccessKey = ConfigurationManager.AppSettings["GumboRealTimeAccessKey"];

            PathLookup = new Dictionary<string, string>();
            PathLookup["Bit Measured Depth"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Hoisting/Main - Drawworks";
            PathLookup["Block Height"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Hoisting/Main - Drawworks";
            PathLookup["Differential Pressure"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["Differential Pressure Set Point"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["Differential Pressure Set Point Active"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["Generator 1 KW"] ="Facility/Rigs/{0}/Industry/Power Generation/Hydraulic Power/Generator";
            PathLookup["Generator 2 KW"] ="Facility/Rigs/{0}/Industry/Power Generation/Hydraulic Power/Generator";
            PathLookup["Generator 3 KW"] ="Facility/Rigs/{0}/Industry/Power Generation/Hydraulic Power/Generator";
            PathLookup["Generator 4 KW"] ="Facility/Rigs/{0}/Industry/Power Generation/Hydraulic Power/Generator";
            PathLookup["Hole Measured Depth"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["Hookload"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Hoisting/Main - Drawworks";
            PathLookup["Pump 1 SPM"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Flow/Main - Mud Pump/Mud Pump Controller";
            PathLookup["Pump 2 SPM"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Flow/Main - Mud Pump/Mud Pump Controller";
            PathLookup["Pump 3 SPM"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Flow/Main - Mud Pump/Mud Pump Controller";
            PathLookup["Left Degrees"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Left Revolutions"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Left RPM"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Left Torque"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Right Degrees"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Right Revolutions"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Right RPM"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Right Torque"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - Rotary Table/Quill Oscillation";
            PathLookup["Rate of Penetration (ROP)"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["Rig State"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["ROP Set Point"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["ROP Set Point Active"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["Stand Pipe Pressure (SPP)"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Additional Sensors/Drilling/Main - Well Center";
            PathLookup["Top Drive Torque"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - AC Top Drive/Top Drive Controller/Signals";
            PathLookup["Top Drive Torque Set Point"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - AC Top Drive/Top Drive Controller/Signals";
            PathLookup["Top Drive Torque Set Point Active"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Rotation/Main - AC Top Drive/Top Drive Controller/Signals";
            PathLookup["Weight On Bit (Calculated)"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Hoisting/Main - Drawworks";
            PathLookup["Weight On Bit Set Point"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Hoisting/Main - Drawworks";
            PathLookup["Weight On Bit Set Point Active"] ="Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Hoisting/Main - Drawworks";

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
        }

        public static string ServerName { get; set; }
        public static string ApiKey { get; set; }
        public static string AccessKey { get; set; }
        public static Dictionary<string, string> PathLookup { get; set; }

        public static long ConvertDate(DateTimeOffset dto)
        {
            long ret = Convert.ToInt64((dto.UtcDateTime - new DateTime(1970, 1, 1)).TotalMilliseconds);
            return ret;
        }

        public static DateTimeOffset ConvertToDate(long ts)
        {
            var timeSpan = TimeSpan.FromMilliseconds(ts);
            var localDateTime = new DateTime(timeSpan.Ticks).ToLocalTime();
            //DateTimeOffset ret = new DateTimeOffset(new DateTime(1970, 1, 1), DateTimeOffset.Now.Offset);
            //ret = ret.AddHours(DateTimeOffset.Now.Offset.Hours);
            //ret = ret.AddMilliseconds(ts);
            DateTimeOffset ret = new DateTimeOffset(localDateTime);
            //return ret;

            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(ts);
            return new DateTimeOffset( dtDateTime).ToLocalTime();
        }

        public IEnumerable<RealTimeDataTuple<T>> Get(string rigNumber, string variableName, DateTimeOffset startDtm, DateTimeOffset endDtm)
        {
            TimeSpan ts = endDtm - startDtm;

            if (ts.TotalHours > 1.01)
            {
                throw new Exception("Timespan between start and end is too long - must be less than an hour");
            }

            List<RealTimeDataTuple<T>> ret = new List<RealTimeDataTuple<T>>();
            const int minutesPerQuery = 15;

            DateTimeOffset dtmSt = startDtm;
            while (dtmSt < endDtm)
            {
                DateTimeOffset dtmEn = dtmSt.AddMinutes(minutesPerQuery);
                if (dtmEn > endDtm) 
                {
                    dtmEn = endDtm;
                }

                string url = string.Format("https://{0}/api/history?apiKey={1}&accessKey={2}&path={3}&from={4}&to={5}", ServerName, ApiKey, AccessKey, GetPathForVariableName(rigNumber, variableName), ConvertDate(dtmSt), ConvertDate(dtmEn));

                url = url.Replace(" ", "%20");
                url = url.Replace("$", "%24");

                //Console.WriteLine("{0}: Submitting url {1}.", DateTimeOffset.Now, url);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                int retries = 1;
                bool successful = false;
                while (retries >= 0 && !successful)
                {
                    try
                    {
                        using (WebResponse webResponse = request.GetResponse())
                        {
                            using (Stream webStream = webResponse.GetResponseStream())
                            {
                                using (StreamReader responseReader = new StreamReader(webStream))
                                {
                                    string response = responseReader.ReadToEnd();
                                    successful = true;
                                    responseReader.Close();
                                    var results = Json.Decode(response);

                                    foreach (DynamicJsonObject djo in results)
                                    {
                                        if (variableName.StartsWith("Generator ") && variableName.EndsWith("KW"))
                                        {
                                            if (djo.GetDynamicMemberNames().Contains("kW") && djo.GetDynamicMemberNames().Contains("Generator ID"))
                                            {
                                                long timestamp = ((dynamic)djo)["timestamp"];
                                                Object o = ((dynamic)djo)["kW"];
                                                T val = (T)Convert.ChangeType(o, typeof(T));
                                                int generatorId = Convert.ToInt32(((dynamic)djo)["Generator ID"]);

                                                if (variableName == string.Format("Generator {0} KW", generatorId))
                                                {
                                                    ret.Add(new RealTimeDataTuple<T>() { Timestamp = ConvertToDate(timestamp), Value = val });
                                                }
                                            }
                                        }
                                        if (variableName.StartsWith("Pump ") && variableName.EndsWith("SPM"))
                                        {
                                            if (djo.GetDynamicMemberNames().Contains("Pump Strokes Per Minute (SPM)") && djo.GetDynamicMemberNames().Contains("Mud Pump ID"))
                                            {
                                                long timestamp = ((dynamic)djo)["timestamp"];
                                                Object o = ((dynamic)djo)["Pump Strokes Per Minute (SPM)"];
                                                T val = (T)Convert.ChangeType(o, typeof(T));
                                                int mudPumpId = Convert.ToInt32(((dynamic)djo)["Mud Pump ID"]);

                                                if (variableName == string.Format("Pump {0} SPM", mudPumpId))
                                                {
                                                    ret.Add(new RealTimeDataTuple<T>() { Timestamp = ConvertToDate(timestamp), Value = val });
                                                }
                                            }
                                        }
                                        else if (djo.GetDynamicMemberNames().Contains(variableName))
                                        {
                                            long timestamp = ((dynamic)djo)["timestamp"];
                                            Object o = ((dynamic)djo)[variableName];
                                            T val = (T)Convert.ChangeType(o, typeof(T));
                                            ret.Add(new RealTimeDataTuple<T>() { Timestamp = ConvertToDate(timestamp), Value = val });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (retries == 0)
                        {
                            Console.WriteLine("{0}: API Failed on url {1}. Error message: {2}. {3} Retries remaining. Throwing Exception.", DateTimeOffset.Now, url, e.Message, retries);
                            throw e;
                        }
                        else
                        {
                            Console.WriteLine("{0}: API Failed on url {1}. Error message: {2}. {3} Retries remaining. Sleeping for 3 seconds.", DateTimeOffset.Now, url, e.Message, retries);
                            Thread.Sleep(3000);
                            retries--;
                        }
                    }
                }

                dtmSt = dtmEn.AddMilliseconds(1);
            }


            return ret;
        }

        public void Put(string rigNumber, string variableName, RealTimeDataTuple<T> data)
        {
            string url = string.Format("https://{0}/api/value?apiKey={1}&accessKey={2}&path={3}&epochtime={4}", ServerName, ApiKey, AccessKey, GetPathForVariableName(rigNumber, variableName), ConvertDate(data.Timestamp));

            url = url.Replace(" ", "%20");
            url = url.Replace("$", "%24");

            string body_content = null;
            if (variableName == "Bit Depth"){
                body_content = "{\"Bit Measured Depth\":" + data.Value + "}";
            } else if (variableName == "Block Height"){
                body_content = "{\"Block Height\":" + data.Value + "}";
            } else if (variableName == "Differential Pressure"){
                body_content = "{\"Differential Pressure\":" + data.Value + "}";
            } else if (variableName == "Differential Pressure Set Point"){
                body_content = "{\"Differential Pressure Set Point\":" + data.Value + "}";
            } else if (variableName == "Differential Pressure Set Point Active"){
                body_content = "{\"Differential Pressure Set Point Active\":" + data.Value.ToString().ToLower() + "}";
            } else if (variableName == "Generator 1 KW"){
                body_content = "{\"Generator ID\":1, \"kW\":" + data.Value + "}";
            } else if (variableName == "Generator 2 KW"){
                body_content = "{\"Generator ID\":2, \"kW\":" + data.Value + "}";
            } else if (variableName == "Generator 3 KW"){
                body_content = "{\"Generator ID\":3, \"kW\":" + data.Value + "}";
            } else if (variableName == "Generator 4 KW"){
                body_content = "{\"Generator ID\":4, \"kW\":" + data.Value + "}";
            }
            else if (variableName == "Hole Depth" || variableName == "Hole Measured Depth")
            {
                body_content = "{\"Hole Measured Depth\":" + data.Value + "}";
            } else if (variableName == "Hookload"){
                body_content = "{\"Hookload\":" + data.Value + "}";
            } else if (variableName == "Pump 1 SPM"){
                body_content = "{\"Pump Strokes Per Minute (SPM)\":" + data.Value + ",\"Mud Pump ID\":1}";
            } else if (variableName == "Pump 2 SPM"){
                body_content = "{\"Pump Strokes Per Minute (SPM)\":" + data.Value + ",\"Mud Pump ID\":2}";
            } else if (variableName == "Pump 3 SPM"){
                body_content = "{\"Pump Strokes Per Minute (SPM)\":" + data.Value + ",\"Mud Pump ID\":3}";
            } else if (variableName == "QO Left Degrees"){
                body_content = "{\"Left Degrees\":" + data.Value + "}";
            } else if (variableName == "QO Left Revolutions"){
                body_content = "{\"Left Revolutions\":" + data.Value + "}";
            } else if (variableName == "QO Left RPM"){
                body_content = "{\"Left RPM\":" + data.Value + "}";
            } else if (variableName == "QO Left Torque"){
                body_content = "{\"Left Torque\":" + data.Value + "}";
            } else if (variableName == "QO Right Degrees"){
                body_content = "{\"Right Degrees\":" + data.Value + "}";
            } else if (variableName == "QO Right Revolutions"){
                body_content = "{\"Right Revolutions\":" + data.Value + "}";
            } else if (variableName == "QO Right RPM"){
                body_content = "{\"Right RPM\":" + data.Value + "}";
            } else if (variableName == "QO Right Torque"){
                body_content = "{\"Right Torque\":" + data.Value + "}";
            } else if (variableName == "Rate of Penetration" || variableName == "Rate of Penetration (ROP)"){
                body_content = "{\"Rate of Penetration (ROP)\":" + data.Value + "}";
            } else if (variableName == "Rig State"){
                body_content = "{\"Rig State\":\"" + data.Value + "\"}";
            } else if (variableName == "ROP Set Point"){
                body_content = "{\"ROP Set Point\":" + data.Value + "}";
            } else if (variableName == "ROP Set Point Active"){
                body_content = "{\"ROP Set Point Active\":" + data.Value.ToString().ToLower() + "}";
            } else if (variableName == "Standpipe Pressure"){
                body_content = "{\"Stand Pipe Pressure (SPP)\":" + data.Value + "}";
            } else if (variableName == "Top Drive Torque"){
                body_content = "{\"Top Drive Torque\":" + data.Value + "}";
            } else if (variableName == "Top Drive Torque Set Point"){
                body_content = "{\"Top Drive Torque Set Point\":" + data.Value + "}";
            } else if (variableName == "Top Drive Torque Set Point Active"){
                body_content = "{\"Top Drive Torque Set Point Active\":" + data.Value.ToString().ToLower() + "}";
            }
            else if (variableName == "Weight On Bit" || variableName == "Weight On Bit (Calculated)")
            {
                body_content = "{\"Weight On Bit (Calculated)\":" + data.Value + "}";
            } else if (variableName == "WOB Set Point"){
                body_content = "{\"Weight On Bit Set Point\":" + data.Value + "}";
            } else if (variableName == "Weight On Bit Set Point Active") {
                body_content = "{\"Weight On Bit Set Point Active\":" + data.Value.ToString().ToLower() + "}";
            }

            if (body_content == null)
            {
                string s = "stop";
            }
            if (body_content != null)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                string gumboPayload = body_content;

                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = gumboPayload.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(gumboPayload);
                requestWriter.Close();

                using (WebResponse webResponse = request.GetResponse())
                {
                    using (Stream webStream = webResponse.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            responseReader.Close();
                        }
                    }
                }
            }
        }

        private static string GetPathForVariableName(string rigNumber, string variableName)
        {
            //return string.Format("Facility/Rigs/{0}/Industry/Oil and Gas/Upstream/Drilling/Hoisting/Main - Drawworks", rigNumber);
            return string.Format(PathLookup[variableName], rigNumber);
        }
    }
}
