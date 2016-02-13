using System;
using System.Collections.Generic;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    public interface ICalendarEventService
    {
        EmployeeRole Role { get; set; }

        IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId, string include = "");
    }
    public class CalendarEventService : ICalendarEventService
    {
        readonly IOrderService _orderService;
        readonly IBranchService _branchService;

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
        public CalendarEventService(IOrderService orderService, IBranchService branchService)
        {
            _orderService = orderService;
            _branchService = branchService;
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
        public IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId, string include = "")
        {
            //var eventStrategy = this.GetAssociateStrategy(role, branchId);

            return EventCalendarStrategy.PopulateEvents(monthInfo, branchId);
        }
    }
}
