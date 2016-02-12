using System.Data.Entity;
using System.Linq;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodCategoryGoodRepository : IGenericRepository<GoodCategoryGood>
    {
        GoodCategoryGood GetById(int branchId, int categoryId);
    }

    public class GoodCategoryGoodRepository : GenericRepository<GoodCategoryGood>, IGoodCategoryGoodRepository
    {
        public GoodCategoryGoodRepository(DbContext context)
              : base(context)
        {

        }
        public GoodCategoryGood GetById(int goodId, int goodCategoryId)
        {
            return FindBy(x => x.GoodId == goodId && x.GoodCategoryId == goodCategoryId).FirstOrDefault();
        }
    }
}
