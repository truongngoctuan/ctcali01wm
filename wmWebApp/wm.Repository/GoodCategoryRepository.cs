using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodCategoryRepository : IGenericRepository<GoodCategory>
    {
    }

    public class GoodCategoryRepository : GenericRepository<GoodCategory>, IGoodCategoryRepository
    {
        public GoodCategoryRepository(DbContext context)
              : base(context)
        {

        }
    }
}
