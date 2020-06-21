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
    public class OperationalRemarkController : ApiController
    {
        public OperationalRemark Get(int id)
        {
            OperationalRemarkService svc = new OperationalRemarkService();
            return svc.GetById(id);
        }

        public OperationalRemark[] Get(string wellId = "")
        {
            OperationalRemarkService svc = new OperationalRemarkService();
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
