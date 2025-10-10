using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvancedProgramming.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        public ActionResult Chat()
        {
            return View("~/Views/Chat/Chat.cshtml");
        }

        public ActionResult Dashboard()
        {
            return View("~/Views/Dashboard/Dashboard.cshtml");
        }

        public ActionResult Documents()
        {
            return View("~/Views/Documents/Documents.cshtml");
        }

        public ActionResult Map()
        {
            return View("~/Views/Map/Map.cshtml");
        }

        public ActionResult Records()
        {
            return View("~/Views/Records/Records.cshtml");
        }
    }
}