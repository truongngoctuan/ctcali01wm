using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodUnitRepository : IGenericIntKeyRepository<GoodUnit>
    {
    }

    public class GoodUnitRepository : GenericIntKeyRepository<GoodUnit>, IGoodUnitRepository
    {
        public GoodUnitRepository(DbContext context)
              : base(context)
        {

        }
    }
}
