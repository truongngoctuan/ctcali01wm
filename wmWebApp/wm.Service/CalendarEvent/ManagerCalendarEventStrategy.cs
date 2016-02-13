using System;
using System.Collections.Generic;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    class ManagerCalendarEventStrategy : CalendarEventStrategyBase
    {
        public ManagerCalendarEventStrategy(IOrderService orderService)
        {
            OrderService = orderService;
        }
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId, string include = "")
        {
            var ordersInMonth = OrderService.GetAllOrdersInMonth(monthInfo, branchId, include);
            return ordersInMonth;
        }
    }
}
