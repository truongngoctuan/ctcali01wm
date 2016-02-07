using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        IEmployeeService _service;
        IEmployeeService Service { get { return _service; } }

        IOrderService _orderService;

        public DashboardController(IEmployeeService Service, IOrderService OrderService)
        {
            _service = Service;
            _orderService = OrderService;
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
            var employee = Service.GetByApplicationId(userId);

            return View(employee);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateEvents(DateTime monthInfo, int branchId)
        {

            var ordersInMonth = _orderService.GetAllOrdersInMonth(monthInfo, branchId);
            var events = ordersInMonth.Select(s => new MonthlyEventItemViewModel
            {
                id = s.Id,
                title = "un known title",
                classs = "event-important",
                start = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                end = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
            });

            var result = new MonthlyEventsViewModel
            {
                success = 1,
                result = events
            };


            return Json(result);
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