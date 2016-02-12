using System;
using System.Linq;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Service.CalendarEvent;
using wm.Web2.Models;

namespace wm.Web2.Controllers.CalendarEventStrategy
{
    public class StaffCalendarEventStrategy : CalendarEventStrategyBase
    {
        public StaffCalendarEventStrategy(
                    ICalendarEventService calendarEventService) : base(calendarEventService)
        {

        }
        public override MonthlyEventsViewModel PopulateEvents(UrlHelper url, DateTime monthInfo, int branchId)
        {
            var events = _calendarEventService.PopulateEvents(monthInfo, branchId);
            var eventsResult = events.Select(s => new CalendarEventItemViewModel
            {
                id = s.Id,
                title = (s.Status == OrderStatus.Started) ? "Do order" : "View order",
                status = s.Status.ToString(),
                url = (s.Status == OrderStatus.Started) ? url.Action("StaffEditOrder", "Orders", new { id = s.Id }) : url.Action("StaffDetailsOrder", "Orders", new { id = s.Id }),
                start = (long)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                end = (long)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
            });

            var result = new MonthlyEventsViewModel
            {
                success = 1,
                result = eventsResult
            };
            return result;
        }
    }
}