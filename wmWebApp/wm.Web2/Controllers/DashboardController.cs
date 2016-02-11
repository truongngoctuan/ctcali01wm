using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Controllers.CalendarEventStrategy;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        IEmployeeService _service;
        IEmployeeService Service { get { return _service; } }

        IOrderService _orderService;
        ICalendarEventService _calendarEventService;
        private readonly IMappingEngine _mapper;

        ICalendarEventControllerHelper _calendarEventStrategy;
        ICalendarEventControllerHelper CalendarEventStrategy
        { 
            get
            {//lazy loading
                if(_calendarEventStrategy == null)
                {
                    var employee = Service.GetByApplicationId(GetUserId());
                    _calendarEventStrategy = new CalendarEventControllerHelper(employee.Role, _calendarEventService);
                }
                return _calendarEventStrategy;
            }
        }
        public DashboardController(ApplicationUserManager userManager,
            ICalendarEventService CalendarEventService,
        IEmployeeService Service, IOrderService OrderService,
        IMappingEngine mapper) : base(userManager)
        {
            _service = Service;
            _orderService = OrderService;
            _calendarEventService = CalendarEventService;
            _mapper = mapper;
        }

        [Authorize]
        public ActionResult GeneralIndex()
        {
            var userId = GetUserId();
            if (UserManager.IsInRole(userId, SystemRoles.Staff))
            {
                return RedirectToAction("StaffDashboard");
            }
            if (UserManager.IsInRole(userId, SystemRoles.Manager))
            {
                return RedirectToAction("BranchManagerIndex");
            }
            if (UserManager.IsInRole(userId, SystemRoles.WarehouseKeeper))
            {
                return RedirectToAction("WhKeeperIndex");
            }
            if (UserManager.IsInRole(userId, SystemRoles.Admin))
            {
                return RedirectToAction("AdminIndex");
            }
            if (UserManager.IsInRole(userId, SystemRoles.SuperUser))
            {
                return RedirectToAction("AdminIndex");
            }
            return View();
        }

        [Authorize]
        public ActionResult StaffDashboard()
        {
            var userId = User.Identity.GetUserId();
            var employee = Service.GetByApplicationId(userId);

            return View(employee);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StaffPopulateEvents(DateTime monthInfo, int branchId)
        {
            return Json(CalendarEventStrategy.PopulateEvents(Url, monthInfo, branchId));
        }

        [Authorize]
        public ActionResult AdminIndex()
        {
            return RedirectToAction("WhKeeperIndex");
        }
        [Authorize]
        public ActionResult BranchManagerIndex()
        {
            return View();
        }
        [Authorize]
        public ActionResult WhKeeperIndex()
        {
            var userId = User.Identity.GetUserId();
            var employee = Service.GetByApplicationId(userId);

            return View(employee);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult WhKeeperPopulateEvents(DateTime monthInfo, int branchId)
        {
            IEnumerable<Order> ordersInMonth = _calendarEventService.PopulateEvents(monthInfo, branchId);
            var eventsResult = ordersInMonth.Select(s => new CalendarEventItemViewModel
            {
                id = s.Id,
                title = (s.Status == OrderStatus.Started) ? "Do order" : "View order",
                status = s.Status.ToString(),
                url = Url.Action("StaffOrder", "Orders", new { id = s.Id }),
            start = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                end = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
            });


            //var eventsResult = _mapper.Map<IEnumerable<CalendarEventItemViewModel>>(events);
            //foreach (var item in eventsResult)
            //{
            //    item.url = Url.Action("StaffOrder", "Orders", new { id = item.id });
            //    //TODO: status
            //}

            var result = new MonthlyEventsViewModel
            {
                success = 1,
                result = eventsResult
            };

            return Json(result);
        }



    }
}