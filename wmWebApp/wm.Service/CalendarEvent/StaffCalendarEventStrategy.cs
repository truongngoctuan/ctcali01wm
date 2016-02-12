using System;
using System.Collections.Generic;
using wm.Model;

namespace wm.Service
{

    class StaffCalendarEventStrategy : CalendarEventStrategyBase
    {
        public StaffCalendarEventStrategy(IOrderService OrderService)
        {
            _orderService = OrderService;
        }
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId)
        {
            var ordersInMonth = _orderService.GetAllOrdersInMonth(monthInfo, branchId);
            return ordersInMonth;
        }
    }

}
