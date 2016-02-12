using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Service.CalendarEvent;
using wm.Web2.Controllers.CalendarEventStrategy;

namespace wm.Web2.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private IEmployeeService Service { get; }

        #region Calendar event strategy

        readonly ICalendarEventService _calendarEventService;
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
                        return new ManagerCalenderEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.StaffBranch:
                    {
                        return new StaffCalendarEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.WarehouseKeeper:
                    {
                        return new WhKeeperCalendarEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.Admin:
                    {
                        return new WhKeeperCalendarEventStrategy(_calendarEventService);
                    }
                case EmployeeRole.SuperUser:
                    {
                        return new WhKeeperCalendarEventStrategy(_calendarEventService);
                    }
            }
            return new StaffCalendarEventStrategy(_calendarEventService);
        }
        #endregion

        public DashboardController(ApplicationUserManager userManager,
            ICalendarEventService calendarEventService,
        IEmployeeService service) : base(userManager)
        {
            Service = service;
            _calendarEventService = calendarEventService;
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
            var userId = User.Identity.GetUserId();
            var employee = Service.GetByApplicationId(userId);

            return View(employee);
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