using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
    }

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context)
              : base(context)
        {

        }
    }
}
