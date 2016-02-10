using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    class WarehouseKeeperEventCalendarStrategy : IEventCalendarStrategyBase
    {
        public WarehouseKeeperEventCalendarStrategy(IOrderService OrderService)
        {
            _orderService = OrderService;
        }
        public override IEnumerable<CalendarEventItem> PopulateEvents(DateTime monthInfo)
        {
            var ordersInMonth = _orderService.GetAllOrdersInMonth(monthInfo);
            var events = ordersInMonth.Select(s => new CalendarEventItem
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
