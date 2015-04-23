using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TwilioRegistration.Frontend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ControlPanel()
        {
            string html = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Content/control-panel.html"));
            return Content(html);
        }
	}
}