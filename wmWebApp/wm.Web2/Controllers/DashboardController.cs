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
        private readonly IMappingEngine _mapper;

        #region Calendar event strategy
        ICalendarEventService _calendarEventService;
        CalendarEventStrategyBase _strategyBase;
        CalendarEventStrategyBase StrategyBase
        {
            get
            {
                if (_strategyBase == null)
                {
                    var employee = Service.GetByApplicationId(GetUserId());
                    _calendarEventService.Role = employee.Role;
                    _strategyBase = GetAssociateStrategy(employee.Role);
                }
                return _strategyBase;
            }
        }


        private CalendarEventStrategyBase GetAssociateStrategy(EmployeeRole role)
        {
            switch (role)
            {
                case EmployeeRole.Manager:
                    {
                        return new StaffCalendarEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.StaffBranch:
                    {
                        return new StaffCalendarEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.WarehouseKeeper:
                    {
                        return new StaffCalendarEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.Admin:
                    {
                        return new StaffCalendarEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.SuperUser:
                    {
                        return new StaffCalendarEventStrategy(_calendarEventService);
                    }
            }
            return new StaffCalendarEventStrategy(_calendarEventService);
        }
        #endregion

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
                return RedirectToAction("ManagerDashboard");
            }
            if (UserManager.IsInRole(userId, SystemRoles.WarehouseKeeper))
            {
                return RedirectToAction("WhKeeperDashboard");
            }
            if (UserManager.IsInRole(userId, SystemRoles.Admin))
            {
                return RedirectToAction("AdminDashboard");
            }
            if (UserManager.IsInRole(userId, SystemRoles.SuperUser))
            {
                return RedirectToAction("AdminDashboard");
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

        [Authorize]
        public ActionResult AdminDashboard()
        {
            return RedirectToAction("WhKeeperDashboard");
        }

        [Authorize]
        public ActionResult ManagerDashboard()
        {
            return View("StaffDashboard");
        }

        [Authorize]
        public ActionResult WhKeeperDashboard()
        {
            var userId = User.Identity.GetUserId();
            var employee = Service.GetByApplicationId(userId);

            return View(employee);
        }

        [HttpPost]
        [Authorize]
        public ActionResult PopulateEvents(DateTime monthInfo, int branchId)
        {
            return Json(StrategyBase.PopulateEvents(Url, monthInfo, branchId));
        }

    }
}