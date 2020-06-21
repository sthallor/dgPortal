using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class Well : BaseEntity
    {
        public int wellId { get; set; }
        public string wellName { get; set; }
        public string uwi { get; set; }
        public int customerId { get; set; }
        public string jobNum { get; set; }
        public string wellNum { get; set; }
        public DateTime? spudDate { get; set; }
        public DateTime? rigReleaseDate { get; set; }
        public string provinceState { get; set; }
        public string country { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int? currentRigId { get; set; }
        public string timeZone { get; set; }
        public bool activeInd { get; set; }
    }
}
