using System;
using System.Collections.Generic;
using System.Web.Mvc;
using wm.Model;
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
        //controller is a Json helper
        public abstract JsonResult PopulateData(int orderId, int goodCategoryId);

        public abstract Order Create(DateTime orderDay, string userId, int branchId);
        public abstract void Place(int orderId, OrderBranchItem[] data);
        public abstract void Confirm(int orderId);
    }
}