using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Service.CalendarEvent;
using wm.Web2.Models;

namespace wm.Web2.Controllers.CalendarEventStrategy
{
    public class ManagerCalenderEventStrategy : CalendarEventStrategyBase
    {
        public ManagerCalenderEventStrategy(ICalendarEventService calendarEventService) : base(calendarEventService)
        {
        }

        public override MonthlyEventsViewModel PopulateEvents(UrlHelper url, DateTime monthInfo, int branchId)
        {
            var orders = _calendarEventService.PopulateEvents(monthInfo, branchId);
            var eventsResult = new List<CalendarEventItemViewModel>();
            foreach (var order in orders)
            {
                var newItem = new CalendarEventItemViewModel();
                newItem.id = order.Id;
                if (order.Priority <= (int) EmployeeRole.Manager)
                {
                    newItem.title = "Confirm order";
                    newItem.url = url.Action("ManagerEditOrder", "Orders", new {id = order.Id});
                }
                else
                {
                    newItem.title = "View order";
                    newItem.url = url.Action("ManagerDetailsOrder", "Orders", new { id = order.Id });
                }
                newItem.status = order.Status.ToString();
                newItem.start = (long) (order.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                newItem.end = (long) (order.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1;

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