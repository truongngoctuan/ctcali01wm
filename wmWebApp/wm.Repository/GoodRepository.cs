using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodRepository : IGenericIntKeyRepository<Good>
    {
    }

    public class GoodRepository : GenericIntKeyRepository<Good>, IGoodRepository
    {
        public GoodRepository(DbContext context)
              : base(context)
        {

        }
    }
}
