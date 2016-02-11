using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers.CalendarEventStrategy
{
    public abstract class CalendarEventStrategyBaseControllerHelper
    {
        protected ICalendarEventService _calendarEventService;
        public CalendarEventStrategyBaseControllerHelper(
                    ICalendarEventService calendarEventService)
        {
            _calendarEventService = calendarEventService;
        }
        public abstract MonthlyEventsViewModel PopulateEvents(UrlHelper Url, DateTime monthInfo, int branchId);
    }
}