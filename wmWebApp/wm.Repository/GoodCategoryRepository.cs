using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodCategoryRepository : IGenericIntKeyRepository<GoodCategory>
    {
    }

    public class GoodCategoryRepository : GenericIntKeyRepository<GoodCategory>, IGoodCategoryRepository
    {
        public GoodCategoryRepository(DbContext context)
              : base(context)
        {

        }
    }
}
