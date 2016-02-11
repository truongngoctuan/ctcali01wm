using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    abstract class IEventCalendarStrategyBase
    {
        protected IOrderService _orderService;
        public abstract IEnumerable<Order> PopulateEvents(DateTime monthInfo);
    }
}
