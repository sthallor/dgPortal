using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataGumboInterface.Service;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterfaceWeb.Controllers
{
    public class CustomerHierarchyController : ApiController
    {
        public CustomerHierarchy[] Get()
        {
            CustomerService svc = new CustomerService();
            IEnumerable<Customer> allCustomers = svc.GetAll();

            List<CustomerHierarchy> ret = allCustomers.Where(c => c.parentCustomerId == null).Select(c => new CustomerHierarchy() { Parent = c }).ToList();

            ret.ForEach(c => AddChildren(c, allCustomers));

            return ret.ToArray();
        }

        private static void AddChildren(CustomerHierarchy ch, IEnumerable<Customer> allCustomers) 
        {
            foreach (Customer cust in allCustomers.Where(c => c.parentCustomerId.HasValue && c.parentCustomerId.Value == ch.Parent.customerId))
            {
                CustomerHierarchy childCust = new CustomerHierarchy() { Parent = cust };
                ch.AddChild(childCust);
                AddChildren(childCust, allCustomers);
            }
        }
    }
}
