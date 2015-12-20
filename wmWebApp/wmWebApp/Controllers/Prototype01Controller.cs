using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wmWebApp.Controllers
{
    public class Prototype01Controller : Controller
    {
        // GET: Prototype01
        public ActionResult Employees()
        {
            return View();
        }

        public ActionResult Agencies()
        {
            return View();
        }

        public ActionResult GoodIndex()
        {
            return View();
        }
    }
}