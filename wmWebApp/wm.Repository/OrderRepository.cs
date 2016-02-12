using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IOrderRepository : IGenericIntKeyRepository<Order>
    {
    }

    public class OrderRepository : GenericIntKeyRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context)
              : base(context)
        {

        }
    }
}
