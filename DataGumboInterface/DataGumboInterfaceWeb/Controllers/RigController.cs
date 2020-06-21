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
    public class RigController : ApiController
    {
        public Rig Get(int id)
        {
            RigService svc = new RigService();
            return svc.GetById(id);
        }

        public Rig[] Get(string rigName = "")
        {
            RigService svc = new RigService();
            if (string.IsNullOrEmpty(rigName))
            {
                return svc.GetAll().ToArray();
            }
            else
            {
                return new Rig[] { svc.GetByRigName(rigName)};
            }
        }
    }
}
