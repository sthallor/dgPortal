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
    public class DrillingAssemblyController : ApiController
    {
        public DrillingAssembly Get(int id)
        {
            DrillingAssemblyService svc = new DrillingAssemblyService();
            return svc.GetById(id);
        }

        public DrillingAssembly[] Get(string wellId = "", string tourSheetId = "")
        {
            DrillingAssemblyService svc = new DrillingAssemblyService();
            if (!string.IsNullOrEmpty(tourSheetId))
            {
                return svc.GetByParentId(Convert.ToInt32(tourSheetId)).ToArray();
            }
            else if (!string.IsNullOrEmpty(wellId))
            {
                List<DrillingAssembly> ret = new List<DrillingAssembly>();
                TourSheetService tsSvc = new TourSheetService();
                tsSvc.GetByParentId(Convert.ToInt32(wellId)).ToList().ForEach(ts => ret.AddRange(svc.GetByParentId(ts.tourSheetId)));
                return ret.ToArray();
            }
            else
            {
                return svc.GetAll().ToArray();
            }
        }
    }
}
