using System;
using System.Collections.Generic;
using wm.Service;
using wm.Service.Model;

namespace wm.Web2.Controllers.OrderStrategy
{
    public abstract class OrderControllerStrategyBase
    {
        protected IOrderService Service { get; }

        protected OrderControllerStrategyBase(IOrderService service)
        {
            Service = service;
        }
        public abstract IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId);

        public abstract void Create(DateTime orderDay, string userId, int branchId);
        public abstract void Confirm(int orderId);
    }
}