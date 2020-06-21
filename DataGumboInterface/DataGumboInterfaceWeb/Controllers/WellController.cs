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
    public class WellController : ApiController
    {
        public Well Get(int id)
        {
            WellService svc = new WellService();
            return svc.GetById(id);
        }

        public Well[] Get(string rigId = "")
        {
            WellService svc = new WellService();
            if (string.IsNullOrEmpty(rigId))
            {
                return svc.GetAll().ToArray();
            }
            else
            {
                return svc.GetByParentId(Convert.ToInt32(rigId)).OrderByDescending(w => w.wellId).ToArray();
            }
        }
    }
}
