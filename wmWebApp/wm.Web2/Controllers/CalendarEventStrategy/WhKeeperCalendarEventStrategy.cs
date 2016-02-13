using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service.CalendarEvent;
using wm.Web2.Models;

namespace wm.Web2.Controllers.CalendarEventStrategy
{
    public class WhKeeperCalendarEventStrategy : CalendarEventStrategyBase
    {
        public WhKeeperCalendarEventStrategy(ICalendarEventService calendarEventService) : base(calendarEventService)
        {
        }

        public override MonthlyEventsViewModel PopulateEvents(UrlHelper url, DateTime monthInfo, int branchId)
        {
            var orders = _calendarEventService.PopulateEvents(monthInfo, branchId);

            //var branchList = 
            var eventsResult = new List<CalendarEventItemViewModel>();
            foreach (var order in orders)
            {
                var newItem = new CalendarEventItemViewModel
                {
                    id = order.Id,
                    status = order.Status.ToString(),
                    start = (long)(order.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                    end = (long)(order.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
                };

                if (order.Priority <= (int)EmployeeRole.WarehouseKeeper)
                {
                    newItem.title = String.Format("{0} - {1}", order.Branch.Name,"Make order");
                    newItem.url = url.Action("WhKeeperEditOrder", "Orders", new { id = order.Id });
                }
                else
                {
                    newItem.title = String.Format("{0} - {1}", order.Branch.Name, "View order");
                    newItem.url = url.Action("WhKeeperDetailsOrder", "Orders", new { id = order.Id });
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