using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class TrippingSpeed : BaseEntity
    {
        public int trippingSpeedId { get; set; }
        public int wellId { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public double activeTime { get; set; }
        public double fromDepth { get; set; }
        public double toDepth { get; set; }
        public string activityDescription { get; set; }
        public string drillerNames { get; set; }
        public string singleOrStands { get; set; }
        public double activeDeltaBlockHeight { get; set; }
        public double activeDeltaBitDepth { get; set; }
        public double activeDistanceTravelled { get; set; }
        public string remark { get; set; }
        public bool bitInCasingInd { get; set; }
        public bool crewHandlingBHAInd { get; set; }
        public string remarkType { get; set; }
        public bool activeInd { get; set; }

    }
}
