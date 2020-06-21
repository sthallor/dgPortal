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
    public class DrillingAssemblyComponentController : ApiController
    {
        public DrillingAssemblyComponent Get(int id)
        {
            DrillingAssemblyComponentService svc = new DrillingAssemblyComponentService();
            return svc.GetById(id);
        }

        public DrillingAssemblyComponent[] Get(string wellId = "", string tourSheetId = "", string drillingAssemblyId = "")
        {
            DrillingAssemblyComponentService svc = new DrillingAssemblyComponentService();
            if (!string.IsNullOrEmpty(drillingAssemblyId))
            {
                return svc.GetByParentId(Convert.ToInt32(drillingAssemblyId)).ToArray();
            }
            else if (!string.IsNullOrEmpty(tourSheetId))
            {
                List<DrillingAssemblyComponent> ret = new List<DrillingAssemblyComponent>();
                DrillingAssemblyService daSvc = new DrillingAssemblyService();
                daSvc.GetByParentId(Convert.ToInt32(tourSheetId)).ToList().ForEach(da => ret.AddRange(svc.GetByParentId(da.drillingAssemblyId)));
                return ret.ToArray();
            }
            else if (!string.IsNullOrEmpty(wellId))
            {
                List<DrillingAssemblyComponent> ret = new List<DrillingAssemblyComponent>();
                TourSheetService tsSvc = new TourSheetService();
                tsSvc.GetByParentId(Convert.ToInt32(wellId)).ToList().ForEach(ts => ret.AddRange(Get(wellId = "", tourSheetId = ts.tourSheetId.ToString(), drillingAssemblyId = "")));
                return ret.ToArray();
            }
            else
            {
                return svc.GetAll().ToArray();
            }
        }
    }
}
