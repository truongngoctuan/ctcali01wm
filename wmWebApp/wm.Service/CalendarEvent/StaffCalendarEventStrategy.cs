using System;
using System.Collections.Generic;
using wm.Model;

namespace wm.Service.CalendarEvent
{

    class StaffCalendarEventStrategy : CalendarEventStrategyBase
    {
        public StaffCalendarEventStrategy(IOrderService orderService)
        {
            OrderService = orderService;
        }
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId)
        {
            var ordersInMonth = OrderService.GetAllOrdersInMonth(monthInfo, branchId);
            return ordersInMonth;
        }
    }

}
