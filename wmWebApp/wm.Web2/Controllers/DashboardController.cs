using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Service;

namespace wm.Web2.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        IEmployeeService _service;
        IEmployeeService Service { get { return _service; } }

        public DashboardController(IEmployeeService Service)
        {
            _service = Service;
        }

        [Authorize]
        public ActionResult GeneralIndex()
        {
            return View();
        }
        [Authorize]
        public ActionResult StaffIndex()
        {
            var userId = User.Identity.GetUserId();
            var employee = Service.GetById(userId);

            return View(employee);
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