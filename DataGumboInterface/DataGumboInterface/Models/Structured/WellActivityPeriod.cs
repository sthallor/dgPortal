using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class WellActivityPeriod : BaseEntity
    {
        public int wellActivityPeriodId { get; set; }
        public int wellId { get; set; }
        public int rigId { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
    }
}
