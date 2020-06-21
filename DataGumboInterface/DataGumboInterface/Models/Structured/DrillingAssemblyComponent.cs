using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class DrillingAssemblyComponent : BaseEntity
    {
        public int drillingAssemblyComponentId { get; set; }
        public int drillingAssemblyId { get; set; }
        public int seqNo { get; set; }
        public string description { get; set; }
        public int componentCount { get; set; }
        public double? maxOutsideDiameter { get; set; }
        public double? minInsideDiameter { get; set; }
        public double? componentLength { get; set; }
        public bool activeInd { get; set; }
    }
}
