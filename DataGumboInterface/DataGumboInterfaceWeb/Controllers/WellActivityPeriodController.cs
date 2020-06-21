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
    public class WellActivityPeriodController : ApiController
    {
        public WellActivityPeriod Get(int id)
        {
            WellActivityPeriodService svc = new WellActivityPeriodService();
            return svc.GetById(id);
        }

        public WellActivityPeriod[] Get(string wellId = "")
        {
            WellActivityPeriodService svc = new WellActivityPeriodService();
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
