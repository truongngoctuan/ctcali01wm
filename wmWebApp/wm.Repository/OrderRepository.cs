using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
