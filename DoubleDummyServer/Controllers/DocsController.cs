using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoubleDummyServer.Controllers
{
    public class DocsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "DDS API Overview";

            return View();
        }

        public ActionResult Api(string id)
        {
            ViewBag.Title = "DDS API";
            return View(id);
        }
    }
}
