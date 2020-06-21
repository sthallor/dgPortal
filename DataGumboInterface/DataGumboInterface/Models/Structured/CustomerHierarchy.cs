using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.Structured
{
    public class CustomerHierarchy
    {
        public Customer Parent { get; set; }
        public List<CustomerHierarchy> Children { get; private set; }

        public CustomerHierarchy()
        {
            Children = new List<CustomerHierarchy>();
        }

        public void AddChild(CustomerHierarchy customer)
        {
            Children.Add(customer);
        }

        public void AddChild(Customer customer)
        {
            AddChild(new CustomerHierarchy() { Parent = customer });
        }
    }
}
