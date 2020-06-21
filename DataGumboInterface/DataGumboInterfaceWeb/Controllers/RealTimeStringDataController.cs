using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataGumboInterface.Models.RealTime;
using DataGumboInterface.Service;
using DataGumboInterfaceWeb.Models;

namespace DataGumboInterfaceWeb.Controllers
{
    public class RealTimeStringDataController : ApiController
    {
        public RealTimeStringDataModel Get(string rigNumber, string variableName)
        {
            RealTimeDataService<string> svc = new RealTimeDataService<string>();
            RealTimeStringDataModel ret = new RealTimeStringDataModel();

            try
            {
                ret.Data = svc.Get(rigNumber, variableName, DateTime.Now.AddMinutes(-15), DateTime.Now).ToArray();
                if (!ret.Data.Any())
                {
                    throw new Exception(string.Format("No data returned for rig {0} and variable {1}", rigNumber, variableName));
                }
                ret.IsError = false;
            }
            catch (Exception e)
            {
                ret.IsError = true;
                ret.ErrorMessage = e.Message;
            }
            return ret;
        }

    }
}
