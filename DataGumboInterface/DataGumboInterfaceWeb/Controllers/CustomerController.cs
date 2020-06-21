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
    public class CustomerController : ApiController
    {
        public Customer Get(int id)
        {
            CustomerService svc = new CustomerService();
            return svc.GetById(id);
        }

        public Customer[] Get(int customerId=0, bool includeParents=false)
        {
            CustomerService svc = new CustomerService();
            if (customerId == 0)
            {
                return svc.GetAll().ToArray();
            }
            else
            {
                Customer c = svc.GetById(customerId);
                if (includeParents)
                {
                    List<Customer> ret = new List<Customer>();
                    ret.Add(c);
                    while (c != null && c.parentCustomerId.HasValue && c.customerId != c.parentCustomerId)
                    {
                        c = svc.GetById(c.parentCustomerId.Value);
                        if (c != null)
                        {
                            ret.Add(c);
                        }
                    }
                    return ret.ToArray();
                }
                else
                {
                    return new [] { c };
                }
            }
        }

        public HttpResponseMessage Post(Customer customer)
        {
            HttpResponseMessage response = response = Request.CreateResponse<Customer>(System.Net.HttpStatusCode.Accepted, Get(customer.customerId));
            CustomerService svc = new CustomerService();
            svc.Update(customer.customerId, customer);
            return response;
        } 
    }
}
