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
    public class TourSheetController : ApiController
    {
        public TourSheet Get(int id)
        {
            TourSheetService svc = new TourSheetService();
            return svc.GetById(id);
        }

        public TourSheet[] Get(string wellId = "")
        {
            TourSheetService svc = new TourSheetService();
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
