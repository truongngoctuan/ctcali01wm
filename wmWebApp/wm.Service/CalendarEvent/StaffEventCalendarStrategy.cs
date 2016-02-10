using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.CalendarEvent
{

    class StaffEventCalendarStrategy : IEventCalendarStrategyBase
    {
        int _branchId;
        public StaffEventCalendarStrategy(IOrderService OrderService,
            int branchId)
        {
            _orderService = OrderService;
            _branchId = branchId;
        }
        public override IEnumerable<EventCalendarItem> PopulateEvents(DateTime monthInfo)
        {
            var ordersInMonth = _orderService.GetAllOrdersInMonth(monthInfo, _branchId);
            var events = ordersInMonth.Select(s => new EventCalendarItem
            {
                id = s.Id,
                title = (s.Status == OrderStatus.Started) ? "Do order" : "View order",
                url = s.Id.ToString(),
                classs = (s.Status == OrderStatus.Started) ? EventColorStatus.Started : EventColorStatus.Finished,
                start = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                end = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
            });

            return events;
        }
    }

}
