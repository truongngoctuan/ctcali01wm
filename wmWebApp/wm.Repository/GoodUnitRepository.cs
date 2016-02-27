using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodUnitRepository : IGenericRepository<GoodUnit>
    {
    }

    public class GoodUnitRepository : GenericRepository<GoodUnit>, IGoodUnitRepository
    {
        public GoodUnitRepository(DbContext context)
              : base(context)
        {

        }
    }
}
