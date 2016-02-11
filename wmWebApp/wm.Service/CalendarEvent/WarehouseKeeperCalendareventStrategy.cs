using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service
{
    class WarehouseKeeperCalendarEventStrategy : CalendarEventStrategyBase
    {
        IBranchService _branchService;
        public WarehouseKeeperCalendarEventStrategy(IOrderService OrderService, IBranchService BranchService)
        {
            _orderService = OrderService;
            _branchService = BranchService;
        }
        public override IEnumerable<Order> PopulateEvents(DateTime monthInfo, int branchId)
        {
            var ordersInMonth = _orderService.GetAllOrdersInMonth(monthInfo);

            //add fake orders
            var fakeOrdersInmonth = new List<Order>();
            var branchList = _branchService.GetAll();
            //for eachday
            var daysInMonth = DateTime.DaysInMonth(monthInfo.Year, monthInfo.Month);
            for (int i = 1; i <= daysInMonth; i++)
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
