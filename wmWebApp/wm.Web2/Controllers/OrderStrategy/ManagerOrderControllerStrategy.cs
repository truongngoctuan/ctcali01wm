using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wm.Model;
using wm.Service;
using wm.Service.Model;

namespace wm.Web2.Controllers.OrderStrategy
{
    public class ManagerOrderControllerStrategy : OrderControllerStrategyBase
    {
        public ManagerOrderControllerStrategy(IOrderService service) : base(service)
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
                Priority = (int)EmployeeRole.Manager
            };

            Service.Create(model);
            return model;
        }

        public override void Place(int orderId, OrderBranchItem[] data)
        {
            var order = Service.GetById(orderId);
            if (!(order.Priority <= (int)EmployeeRole.Manager)) return;

            Service.Place(orderId, data);

            order.Priority = (int)EmployeeRole.Manager;
            Service.Update(order);
        }

        public override void Confirm(int orderId)
        {
            //TODO: check permission
            var order = Service.GetById(orderId);
            order.Priority = (int) EmployeeRole.WarehouseKeeper;
            Service.Update(order);
            Service.ChangeStatus(orderId, OrderStatus.ManagerConfirmed);
        }
    }
}