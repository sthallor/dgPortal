using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public abstract class BaseEntity
    {
        public string rowCreatedBy { get; set; }
        public string rowChangedBy { get; set; }
        public DateTime? rowCreatedDate { get; set; }
        public DateTime? rowChangedDate { get; set; }
    }
}
