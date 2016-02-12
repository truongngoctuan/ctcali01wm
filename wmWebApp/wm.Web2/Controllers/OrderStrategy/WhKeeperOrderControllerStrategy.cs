using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wm.Model;
using wm.Service;
using wm.Service.Model;

namespace wm.Web2.Controllers.OrderStrategy
{
    public class WhKeeperOrderControllerStrategy : OrderControllerStrategyBase
    {
        public WhKeeperOrderControllerStrategy(IOrderService service) : base(service)
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
                Priority = (int)EmployeeRole.WarehouseKeeper
            };

            Service.Create(model);
            return model;
        }

        public override void Place(int orderId, OrderBranchItem[] data)
        {
            var order = Service.GetById(orderId);
            if (!(order.Priority <= (int)EmployeeRole.WarehouseKeeper)) return;

            Service.Place(orderId, data);

            order.Priority = (int)EmployeeRole.WarehouseKeeper;
            Service.Update(order);
        }

        public override void Confirm(int orderId)
        {
            //TODO: check permission
            var order = Service.GetById(orderId);
            order.Priority = (int)EmployeeRole.Admin;
            Service.Update(order);
            Service.ChangeStatus(orderId, OrderStatus.Finished);
        }
    }
}