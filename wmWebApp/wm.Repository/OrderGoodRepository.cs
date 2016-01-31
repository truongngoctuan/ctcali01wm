using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IOrderGoodRepository : IGenericRepository<OrderGood>
    {
        OrderGood GetById(int orderId, int goodId);
    }

    public class OrderGoodRepository : GenericRepository<OrderGood>, IOrderGoodRepository
    {
        public OrderGoodRepository(DbContext context)
              : base(context)
        {

        }
        public OrderGood GetById(int orderId, int goodId)
        {
            return FindBy(x => x.OrderId == orderId && x.GoodId == goodId).FirstOrDefault();
        }
    }
}
