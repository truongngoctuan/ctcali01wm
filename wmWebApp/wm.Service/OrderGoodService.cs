using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Service.Common;

namespace wm.Service
{
    public interface IOrderGoodService : IEntityService<OrderGood>
    {
        IEnumerable<OrderGood> GetByOrderId(int orderId, string include = "");
        IEnumerable<OrderGood> GetByOrderIdRange(IEnumerable<int> orderIds, GoodType? type = null);
    }

    public class OrderGoodService : EntityService<OrderGood>, IOrderGoodService
    {
        public OrderGoodService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

        public IEnumerable<OrderGood> GetByOrderId(int orderId, string include = "")
        {
            return Get((s => s.OrderId == orderId), include);
        }

        public IEnumerable<OrderGood> GetByOrderIdRange(IEnumerable<int> orderIds, GoodType? type = null)
        {
            var result = _dbset.Where(s => orderIds.Contains(s.OrderId)).Include("Good");
            if (type != null)
            {
                result = result.Where(s => s.Good.GoodType == type);
            }
            return result;
        }
    }
}
