using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataGumboInterface.Models.RealTime;

namespace DataGumboInterfaceWeb.Models
{
    public class RealTimeStringDataModel
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public RealTimeDataTuple<string>[] Data { get; set;}
    }
}