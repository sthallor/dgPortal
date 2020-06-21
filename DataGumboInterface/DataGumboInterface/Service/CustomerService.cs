using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterface.Service
{
    public class CustomerService : StructuredDataService<Customer>
    {
        public override string GumboTypeName
        {
            get { return "customer"; }
        }
    }
}
