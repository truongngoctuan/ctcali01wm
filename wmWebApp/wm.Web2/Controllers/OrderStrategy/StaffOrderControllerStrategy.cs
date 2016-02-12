using System;
using System.Collections.Generic;
using wm.Model;
using wm.Service;
using wm.Service.Model;

namespace wm.Web2.Controllers.OrderStrategy
{
    public class StaffOrderControllerStrategy : OrderControllerStrategyBase
    {
        public StaffOrderControllerStrategy(IOrderService service): base(service)
        {
            
        }
        public override IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId)
        {
            return Service.PopulateData(orderId, goodCategoryId);
        }

        public override void Create(DateTime orderDay, string userId, int branchId)
        {
            var model = new Order
            {
                BranchId = branchId,
                OrderDay = orderDay,
                CreatedBy = userId,
                Status = OrderStatus.Started,
                Priority = 0
            };

            Service.Create(model);
        }

        public override void Confirm(int orderId)
        {
            Service.ChangeStatus(orderId, OrderStatus.StaffConfirmed);
        }
    }
}