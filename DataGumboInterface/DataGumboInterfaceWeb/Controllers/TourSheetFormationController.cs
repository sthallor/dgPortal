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
    public class TourSheetFormationController : ApiController
    {
        public TourSheetFormation Get(int id)
        {
            TourSheetFormationService svc = new TourSheetFormationService();
            return svc.GetById(id);
        }

        public TourSheetFormation[] Get(string wellId = "", string tourSheetId = "")
        {
            TourSheetFormationService svc = new TourSheetFormationService();
            if (!string.IsNullOrEmpty(tourSheetId))
            {
                return svc.GetByParentId(Convert.ToInt32(tourSheetId)).ToArray();
            }
            else if (!string.IsNullOrEmpty(wellId))
            {
                List<TourSheetFormation> ret = new List<TourSheetFormation>();
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
