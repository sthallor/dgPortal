using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class TourSheetFormation : BaseEntity
    {
        public int tourSheetFormationId { get; set; }
        public int tourSheetId { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public string formationName { get; set; }
        public bool activeInd { get; set; }
    }
}
