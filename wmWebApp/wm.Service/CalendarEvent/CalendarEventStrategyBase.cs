using System;
using System.Collections.Generic;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    abstract class CalendarEventStrategyBase
    {
        protected IOrderService OrderService { get; set; }
        public abstract IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId, string include = "");
    }
}
