using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class Rig : BaseEntity
    {
        public int rigId { get; set; }
        public string rigName { get; set; }
        public bool activeInd { get; set; }
    }
}
