using System;
using System.Collections.Generic;
using System.Web.Mvc;
using wm.Model;
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
            var orders = _calendarEventService.PopulateEvents(monthInfo, branchId);
            var eventsResult = new List<CalendarEventItemViewModel>();
            foreach (var order in orders)
            {
                var newItem = new CalendarEventItemViewModel
                {
                    id = order.Id,
                    status = order.Status.ToString(),
                    start = (long) (order.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                    end = (long) (order.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
                };

                if (order.Priority <= (int)EmployeeRole.StaffBranch)
                {
                    newItem.title = "Make order";
                    newItem.url = url.Action("StaffEditOrder", "Orders", new { id = order.Id });
                }
                else
                {
                    newItem.title = "View order";
                    newItem.url = url.Action("StaffDetailsOrder", "Orders", new { id = order.Id });
                }


                eventsResult.Add(newItem);
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