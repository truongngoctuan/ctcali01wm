using System.Collections.Generic;
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
    }
}