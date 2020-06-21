using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class Customer : BaseEntity
    {
        public int customerId { get; set; }
        public string customerName { get; set; }
        public bool activeInd { get; set; }
        public int? parentCustomerId { get; set; }
        public string pbiApplicationId { get; set; }
        public string pbiWorkspaceId { get; set; }
        public string pbiUser { get; set; }
        public string pbiPassword { get; set; }

    }
}
