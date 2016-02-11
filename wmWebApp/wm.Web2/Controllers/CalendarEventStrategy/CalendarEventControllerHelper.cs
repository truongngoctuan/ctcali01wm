using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers.CalendarEventStrategy
{
    public interface ICalendarEventControllerHelper
    {
        EmployeeRole Role { set; }
        bool isInit();

        MonthlyEventsViewModel PopulateEvents(UrlHelper Url, DateTime monthInfo, int branchId);
    }

    public class CalendarEventControllerHelper :ICalendarEventControllerHelper
    {
        public EmployeeRole Role {
            set {
                _calendarEventService.Role = value;
                _strategyBase = GetAssociateStrategy(value);
            }
        }
        CalendarEventStrategyBaseControllerHelper _strategyBase;

        ICalendarEventService _calendarEventService;
        public CalendarEventControllerHelper(ICalendarEventService calendarEventService)
        {
            _calendarEventService = calendarEventService;
        }

        public bool isInit()
        {
            return (_strategyBase != null);
        }

        private CalendarEventStrategyBaseControllerHelper GetAssociateStrategy(EmployeeRole role)
        {
            switch (role)
            {
                case EmployeeRole.Manager:
                    {
                        return new StaffCalendarEventStrategyControllerHelper(_calendarEventService);
                    }
                case EmployeeRole.StaffBranch:
                    {
                        return new StaffCalendarEventStrategyControllerHelper(_calendarEventService);
                    }
                case EmployeeRole.WarehouseKeeper:
                    {
                        return new StaffCalendarEventStrategyControllerHelper(_calendarEventService);
                    }
                case EmployeeRole.Admin:
                    {
                        return new StaffCalendarEventStrategyControllerHelper(_calendarEventService);
                    }
                case EmployeeRole.SuperUser:
                    {
                        return new StaffCalendarEventStrategyControllerHelper(_calendarEventService);
                    }
            }
            return new StaffCalendarEventStrategyControllerHelper(_calendarEventService);
        }

        public MonthlyEventsViewModel PopulateEvents(UrlHelper Url, DateTime monthInfo, int branchId)
        {
            return _strategyBase.PopulateEvents(Url, monthInfo, branchId);
        }


    }
}