using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Service.CalendarEvent;
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
            IEnumerable<CalendarEventItem> events = _calendarEventService.PopulateEvents(EmployeeRole.StaffBranch, monthInfo, branchId);

            var eventsResult = _mapper.Map<IEnumerable<CalendarEventItemViewModel>>(events);

            //var eventsResult = events.Select(s => new CalendarEventItemViewModel
            //{
            //    id = s.id,
            //    title = s.title,
            //    url = Url.Action("StaffOrder", "Orders", new { id = s.id }),
            //    classs = s.classs,
            //    start = s.start,
            //    end = s.end
            //});

            var result = new MonthlyEventsViewModel
            {
                success = 1,
                result = eventsResult
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
        public ActionResult WhKeeperIndex()
        {
            return View();
        }

        
    }
}