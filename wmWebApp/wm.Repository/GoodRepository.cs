using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodRepository : IGenericRepository<Good>
    {
    }

    public class GoodRepository : GenericRepository<Good>, IGoodRepository
    {
        public GoodRepository(DbContext context)
              : base(context)
        {

        }
    }
}
