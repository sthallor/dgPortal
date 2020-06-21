using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataGumboInterfaceWeb.Models;

namespace DataGumboInterfaceWeb.Controllers
{
    public class HomeController : Controller
    {
        private void SetEnvironmentVariable()
        {
            ViewBag.Environment = ConfigurationManager.AppSettings["Environment"];
        }
        public ActionResult Index(HomeModel model)
        {
            SetEnvironmentVariable();
            return View(model);
        }
        public ActionResult CustomerHierarchy(HomeModel model)
        {
            SetEnvironmentVariable();
            return View(model);
        }
        public ActionResult CustomerEdit(HomeModel model)
        {
            SetEnvironmentVariable();
            return View(model);
        }
        public ActionResult RealTime(HomeModel model)
        {
            SetEnvironmentVariable();
            return View(model);
        }
        public ActionResult Rig(HomeModel model)
        {
            SetEnvironmentVariable();
            return View(model);
        }
    }
}
