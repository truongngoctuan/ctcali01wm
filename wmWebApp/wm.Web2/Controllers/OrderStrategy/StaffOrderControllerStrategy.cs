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

        public override Order Create(DateTime orderDay, string userId, int branchId)
        {
            var model = new Order
            {
                BranchId = branchId,
                OrderDay = orderDay,
                CreatedBy = userId,
                Status = OrderStatus.Started,
                Priority = (int)EmployeeRole.StaffBranch
            };

            Service.Create(model);
            return model;
        }

        public override void Place(int orderId, OrderBranchItem[] data)
        {
            var order = Service.GetById(orderId);
            if (!(order.Priority <= (int) EmployeeRole.StaffBranch)) return;

            Service.Place(orderId, data);

            order.Priority = (int)EmployeeRole.StaffBranch;
            Service.Update(order);
        }

        public override void Confirm(int orderId)
        {
            //TODO: check permission
            var order = Service.GetById(orderId);
            order.Priority = (int)EmployeeRole.Manager;
            Service.Update(order);
            Service.ChangeStatus(orderId, OrderStatus.StaffConfirmed);
        }
    }
}