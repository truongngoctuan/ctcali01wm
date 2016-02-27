using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;

namespace wm.Repository
{
    public interface IOrderGoodRepository : IGenericRepository<OrderGood>
    {
        IEnumerable<OrderGood> GetByOrderIdRange(IEnumerable<int> orderIds, GoodType? type = null);
    }

    public class OrderGoodRepository : GenericRepository<OrderGood>, IOrderGoodRepository
    {
        public OrderGoodRepository(DbContext context)
              : base(context)
        {

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
