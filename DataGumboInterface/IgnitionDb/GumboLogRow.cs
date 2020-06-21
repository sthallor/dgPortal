using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgnitionDb
{
    public class GumboLogRow
    {
        public int logId { get; set; }
        public DateTime logDtm { get; set; }
        public string path { get; set; }
        public string rig { get; set; }
        public string value { get; set; }
        public double? numericValue
        {
            get
            {
                double d = 0;
                if (Double.TryParse(value, out d))
                {
                    return d;
                }
                return null;
            }
        }
        public string tagName
        {
            get
            {
                string ret = path.Split("()".ToCharArray())[1];

                if (ret == "Bit Depth")
                {
                    return "Bit Measured Depth";
                }
                else if (ret == "Hole Depth")
                {
                    return "Hole Measured Depth";
                }
                else if (ret == "Standpipe Pressure")
                {
                    return "Stand Pipe Pressure (SPP)";
                }
                else if (ret == "Weight On Bit")
                {
                    return "Weight On Bit (Calculated)";
                }
                else if (ret == "Rate of Penetration")
                {
                    return "Rate of Penetration (ROP)";
                }
                else if (ret == "QO Left Degrees")
                {
                    return "Left Degrees";
                }
                else if (ret == "QO Left RPM")
                {
                    return "Left RPM";
                }
                else if (ret == "QO Left Torque")
                {
                    return "Left Torque";
                }
                else if (ret == "QO Right Revolutions")
                {
                    return "Right Revolutions";
                }
                else if (ret == "QO Left Revolutions")
                {
                    return "Left Revolutions";
                }
                else if (ret == "QO Right Degrees")
                {
                    return "Right Degrees";
                }
                else if (ret == "QO Right RPM")
                {
                    return "Right RPM";
                }
                else if (ret == "QO Right Torque")
                {
                    return "Right Torque";
                }
                else if (ret == "WOB Set Point")
                {
                    return "Weight On Bit Set Point";
                }
                return ret;
            }
        }
    }
}
