using System.Data.Entity;
using System.Linq;
using wm.Model;

namespace wm.Repository
{
    public interface IBranchGoodCategoryRepository : IGenericRepository<BranchGoodCategory>
    {
    }

    public class BranchGoodCategoryRepository : GenericRepository<BranchGoodCategory>, IBranchGoodCategoryRepository
    {
        public BranchGoodCategoryRepository(DbContext context)
              : base(context)
        {

        }
    }
}
