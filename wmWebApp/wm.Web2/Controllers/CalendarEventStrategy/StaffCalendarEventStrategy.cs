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
    public class StaffCalendarEventStrategy : CalendarEventStrategyBase
    {
        public StaffCalendarEventStrategy(
                    ICalendarEventService calendarEventService) : base(calendarEventService)
        {

        }
        public override MonthlyEventsViewModel PopulateEvents(UrlHelper Url, DateTime monthInfo, int branchId)
        {
            IEnumerable<Order> events = _calendarEventService.PopulateEvents(monthInfo, branchId);
            var eventsResult = events.Select(s => new CalendarEventItemViewModel
            {
                id = s.Id,
                title = (s.Status == OrderStatus.Started) ? "Do order" : "View order",
                status = s.Status.ToString(),
                //TODO: auto redirect to StaffViewOrder
                url = Url.Action("StaffOrder", "Orders", new { id = s.Id }),
                start = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                end = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
            });

            //var eventsResult = _mapper.Map<IEnumerable<CalendarEventItemViewModel>>(events);
            foreach (var item in eventsResult)
            {
                item.url = Url.Action("StaffOrder", "Orders", new { id = item.id });
            }

            var result = new MonthlyEventsViewModel
            {
                success = 1,
                result = eventsResult
            };
            return result;
        }
    }
}