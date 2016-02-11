﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    class ManagerEventCalendarStrategy : IEventCalendarStrategyBase
    {
        public ManagerEventCalendarStrategy(IOrderService OrderService)
        {
            _orderService = OrderService;
        }
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo)
        {
            throw new NotImplementedException();
        }
    }
}
