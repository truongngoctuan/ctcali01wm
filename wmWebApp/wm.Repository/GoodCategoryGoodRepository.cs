using System.Data.Entity;
using System.Linq;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodCategoryGoodRepository : IGenericRepository<GoodCategoryGood>
    {
    }

    public class GoodCategoryGoodRepository : GenericRepository<GoodCategoryGood>, IGoodCategoryGoodRepository
    {
        public GoodCategoryGoodRepository(DbContext context)
              : base(context)
        {

        }
    }
}
