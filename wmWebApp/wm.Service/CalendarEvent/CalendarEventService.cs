using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service
{
    public interface ICalendarEventService
    {
        EmployeeRole Role { get; set; }

        IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId);
    }
    public class CalendarEventService : ICalendarEventService
    {
        IOrderService _orderService;
        IBranchService _branchService;

        public EmployeeRole Role { get; set; }

        CalendarEventStrategyBase _eventCalendarStrategy;
        CalendarEventStrategyBase EventCalendarStrategy
        {
            get
            {
                if (_eventCalendarStrategy == null) { //init
                    _eventCalendarStrategy = GetAssociateStrategy(Role);
                }
                return _eventCalendarStrategy;
            }
        }
        public CalendarEventService(IOrderService OrderService, IBranchService BranchService)
        {
            _orderService = OrderService;
            _branchService = BranchService;
        }

        private CalendarEventStrategyBase GetAssociateStrategy(EmployeeRole role)
        {
            switch (role)
            {
                case EmployeeRole.Manager:
                    {
                        return new ManagerCalendarEventStrategy(_orderService);
                    }
                case EmployeeRole.StaffBranch:
                    {
                        return new StaffCalendarEventStrategy(_orderService);
                    }
                case EmployeeRole.WarehouseKeeper:
                    {
                        return new WarehouseKeeperCalendarEventStrategy(_orderService, _branchService);
                    }
                case EmployeeRole.Admin:
                    {
                        return new WarehouseKeeperCalendarEventStrategy(_orderService, _branchService);
                    }
                case EmployeeRole.SuperUser:
                    {
                        return new WarehouseKeeperCalendarEventStrategy(_orderService, _branchService);
                    }
            }
            return new StaffCalendarEventStrategy(_orderService);
        }
        public IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId)
        {
            //var eventStrategy = this.GetAssociateStrategy(role, branchId);

            return EventCalendarStrategy.PopulateEvents(monthInfo, branchId);
        }
    }
}
