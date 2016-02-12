using System;
using System.Collections.Generic;
using wm.Model;

namespace wm.Service
{
    class ManagerCalendarEventStrategy : CalendarEventStrategyBase
    {
        public ManagerCalendarEventStrategy(IOrderService OrderService)
        {
            _orderService = OrderService;
        }
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId)
        {
            throw new NotImplementedException();
        }
    }
}
