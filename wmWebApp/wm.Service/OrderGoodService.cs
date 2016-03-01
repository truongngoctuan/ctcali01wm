using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IOrderGoodService : IEntityService<OrderGood>
    {
        IEnumerable<OrderGood> GetByOrderId(int orderId, string include = "");
        IEnumerable<OrderGood> GetByOrderIdRange(IEnumerable<int> orderIds, GoodType? type = null);
    }

    public class OrderGoodService : EntityService<OrderGood>, IOrderGoodService
    {
        readonly IOrderGoodRepository _repos2;

        public OrderGoodService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
            _repos2 = new OrderGoodRepository(context);
        }

        public IEnumerable<OrderGood> GetByOrderId(int orderId, string include = "")
        {
            return Get((s => s.OrderId == orderId), include);
        }

        public IEnumerable<OrderGood> GetByOrderIdRange(IEnumerable<int> orderIds, GoodType? type = null)
        {
            return _repos2.GetByOrderIdRange(orderIds, type);
        }
    }
}
