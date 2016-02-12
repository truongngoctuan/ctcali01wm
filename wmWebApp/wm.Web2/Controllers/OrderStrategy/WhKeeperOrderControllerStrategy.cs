using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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

        public override JsonResult PopulateData(int orderId, int goodCategoryId)
        {
            var order = Service.GetById(orderId, "Branch");
            if (order.Branch.BranchType == BranchType.MainKitchen)
            {
                return PopulateDataMainKitchen(orderId, goodCategoryId);
            }
            else
            {
                return PopulateDataNormalBranch(orderId, goodCategoryId);
            }
        }

        private JsonResult PopulateDataNormalBranch(int orderId, int goodCategoryId)
        {
            var items = Service.PopulateData(orderId, goodCategoryId);
            return new JsonResult() { Data = items };
        }
        private JsonResult PopulateDataMainKitchen(int orderId, int goodCategoryId)
        {
            var items = Service.PopulateData(orderId, goodCategoryId);
            return new JsonResult() { Data = items };
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