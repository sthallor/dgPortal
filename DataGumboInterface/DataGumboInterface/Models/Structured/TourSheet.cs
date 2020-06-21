using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class TourSheet : BaseEntity
    {
        public int tourSheetId { get; set; }
        public int wellId { get; set; }
        public int customerId { get; set; }
        public int seqNo { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public string contractRepName { get; set; }
        public string operatorRepName { get; set; }
        public int? rigId { get; set; }
        public bool activeInd { get; set; }
    }
}
