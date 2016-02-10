using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    public interface ICalendarEventService
    {
        IEnumerable<EventCalendarItem> PopulateEvents(EmployeeRole role, DateTime monthInfo, int branchId);
    }
    public class CalendarEventService : ICalendarEventService
    {
        IOrderService _orderService;
        public CalendarEventService(IOrderService OrderService)
        {
            _orderService = OrderService;
        }

        private IEventCalendarStrategyBase GetAssociateStrategy(EmployeeRole role, int branchId)
        {
            switch(role)
            {
                case EmployeeRole.Manager:
                    {
                        return new ManagerEventCalendarStrategy(_orderService);
                    }
                case EmployeeRole.StaffBranch:
                    {
                        return new StaffEventCalendarStrategy(_orderService, branchId);
                    }
                case EmployeeRole.WarehouseKeeper:
                    {
                        return new WarehouseKeeperEventCalendarStrategy(_orderService);
                    }
                case EmployeeRole.Admin:
                    {
                        return new WarehouseKeeperEventCalendarStrategy(_orderService);
                    }
                case EmployeeRole.SuperUser:
                    {
                        return new WarehouseKeeperEventCalendarStrategy(_orderService);
                    }
            }
            return new StaffEventCalendarStrategy(_orderService, branchId);
        }
        public IEnumerable<EventCalendarItem> PopulateEvents(EmployeeRole role, DateTime monthInfo, int branchId)
        {
            var eventStrategy = this.GetAssociateStrategy(role, branchId);

            return eventStrategy.PopulateEvents(monthInfo);
        }
    }
}
