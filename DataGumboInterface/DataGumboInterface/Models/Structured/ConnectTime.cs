using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class ConnectTime : BaseEntity
    {
        public int connectTimeId { get; set; }
        public int wellId { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public double fromDepth { get; set; }
        public double toDepth { get; set; }
        public string activityDescription { get; set; }
        public string drillerNames { get; set; }
        public string singleOrStands { get; set; }
        public int numberOfConnections { get; set; }
        public double avgWtoWConnectTime { get; set; }
        public double avgStoSConnectTime { get; set; }
        public double avgReamCirculateConditionTime { get; set; }
        public double avgSurveyTime { get; set; }
        public string remark { get; set; }
        public bool activeInd { get; set; }
    }
}
