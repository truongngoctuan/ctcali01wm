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
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo)
        {
            var ordersInMonth = _orderService.GetAllOrdersInMonth(monthInfo, _branchId);
            return ordersInMonth;
        }
    }

}
