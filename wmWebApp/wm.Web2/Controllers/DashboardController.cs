using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wm.Web2.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [Authorize]
        public ActionResult GeneralIndex()
        {
            return View();
        }
        [Authorize]
        public ActionResult StaffIndex()
        {
            return View();
        }
        [Authorize]
        public ActionResult AdminIndex()
        {
            return View();
        }
        [Authorize]
        public ActionResult BranchManagerIndex()
        {
            return View();
        }
        [Authorize]
        public ActionResult StockIndex()
        {
            return View();
        }
    }
}