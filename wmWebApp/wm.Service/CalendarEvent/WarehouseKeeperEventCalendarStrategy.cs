﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    class WarehouseKeeperEventCalendarStrategy : IEventCalendarStrategyBase
    {
        IBranchService _branchService;
        public WarehouseKeeperEventCalendarStrategy(IOrderService OrderService, IBranchService BranchService)
        {
            _orderService = OrderService;
            _branchService = BranchService;
        }
        public override IEnumerable<CalendarEventItem> PopulateEvents(DateTime monthInfo)
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

            var events = fakeOrdersInmonth.Select(s => new CalendarEventItem
            {
                id = s.Id,
                title = (s.Status == OrderStatus.NotStarted) ? "Do order" : "Confirm order",
                url = s.Id.ToString(),
                classs = (s.Status == OrderStatus.NotStarted) ? EventColorStatus.NotStarted : EventColorStatus.Started,
                start = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
                end = (Int64)(s.OrderDay.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds + 1
            });

            return events;
        }
    }
}