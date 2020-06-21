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
    public class ConnectTimeController : ApiController
    {
        public ConnectTime Get(int id)
        {
            ConnectTimeService svc = new ConnectTimeService();
            return svc.GetById(id);
        }

        public ConnectTime[] Get(string wellId = "")
        {
            ConnectTimeService svc = new ConnectTimeService();
            if (string.IsNullOrEmpty(wellId))
            {
                return svc.GetAll().ToArray();
            }
            else
            {
                return svc.GetByParentId(Convert.ToInt32(wellId)).ToArray();
            }
        }
    }
}
