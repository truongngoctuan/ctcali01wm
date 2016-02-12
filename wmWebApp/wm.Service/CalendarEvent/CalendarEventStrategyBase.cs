using System;
using System.Collections.Generic;
using wm.Model;

namespace wm.Service
{
    abstract class CalendarEventStrategyBase
    {
        protected IOrderService _orderService;
        public abstract IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId);
    }
}
