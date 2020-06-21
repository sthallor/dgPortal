using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class OperationalRemark : BaseEntity
    {
        public int operationalRemarkId { get; set; }
        public int tourSheetId { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public string remarkType { get; set; }
        public string remark { get; set; }
        public string remarkCode { get; set; }
        public bool activeInd { get; set; }
    }
}
