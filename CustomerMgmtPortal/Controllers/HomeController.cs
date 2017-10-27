using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerMgmtPortal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoToHome()
        {
            ViewData["CurrentTime"] = DateTime.Now.ToString();
            return View("MyHomePage");
        }
    }
}