using System;
using System.Web.Mvc;
using wm.Service;
using wm.Service.CalendarEvent;
using wm.Web2.Models;

namespace wm.Web2.Controllers.CalendarEventStrategy
{
    public abstract class CalendarEventStrategyBase
    {
        protected ICalendarEventService _calendarEventService;
        public CalendarEventStrategyBase(
                    ICalendarEventService calendarEventService)
        {
            _calendarEventService = calendarEventService;
        }
        public abstract MonthlyEventsViewModel PopulateEvents(UrlHelper url, DateTime monthInfo, int branchId);
    }
}