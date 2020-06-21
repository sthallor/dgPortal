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
    public class TrippingSpeedController : ApiController
    {
        public TrippingSpeed Get(int id)
        {
            TrippingSpeedService svc = new TrippingSpeedService();
            return svc.GetById(id);
        }

        public TrippingSpeed[] Get(string wellId = "")
        {
            TrippingSpeedService svc = new TrippingSpeedService();
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
