using System;
using System.Collections.Generic;
using System.Linq;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    class WarehouseKeeperCalendarEventStrategy : CalendarEventStrategyBase
    {
        readonly IBranchReadOnlyService _branchReadOnlyService;
        public WarehouseKeeperCalendarEventStrategy(IOrderService orderService, IBranchReadOnlyService branchReadOnlyService)
        {
            OrderService = orderService;
            _branchReadOnlyService = branchReadOnlyService;
        }
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId, string include = "")
        {
            var ordersInMonth = OrderService.GetAllOrdersInMonth(monthInfo, 0, include).ToList();

            //add fake orders
            var fakeOrdersInmonth = new List<Order>();
            var branchList = _branchReadOnlyService.GetAll().ToList();
            //for eachday
            var daysInMonth = DateTime.DaysInMonth(monthInfo.Year, monthInfo.Month);
            for (var i = 1; i <= daysInMonth; i++)
            {
                foreach(var branch in branchList)
                {
                    var matches = ordersInMonth.Where(s => s.BranchId == branch.Id && s.OrderDay.Day == i);
                    if (matches.Any())
                    {
                        fakeOrdersInmonth.AddRange(matches);
                    }
                    else
                    {
                        //create fake order 
                        fakeOrdersInmonth.Add(new Order
                        {
                            Id = 0,
                            BranchId = branch.Id,
                            Branch = branch,
                            OrderDay = new DateTime(monthInfo.Year, monthInfo.Month, i),
                            Priority = 0,
                            Status = OrderStatus.NotStarted
                        });
                    }
                }
            }

            return fakeOrdersInmonth;
        }
    }
}
