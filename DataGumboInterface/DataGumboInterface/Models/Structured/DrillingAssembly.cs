using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class DrillingAssembly : BaseEntity
    {
        public int drillingAssemblyId { get; set; }
        public int tourSheetId { get; set; }
        public int seqNo { get; set; }
        public double totalLength { get; set; }
        public double totalWeight { get; set; }
        public bool activeInd { get; set; }
    }
}
