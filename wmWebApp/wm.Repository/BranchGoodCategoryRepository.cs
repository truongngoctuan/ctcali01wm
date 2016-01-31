using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IBranchGoodCategoryRepository : IGenericRepository<BranchGoodCategory>
    {
        BranchGoodCategory GetById(int branchId, int categoryId);
    }

    public class BranchGoodCategoryRepository : GenericRepository<BranchGoodCategory>, IBranchGoodCategoryRepository
    {
        public BranchGoodCategoryRepository(DbContext context)
              : base(context)
        {

        }
        public BranchGoodCategory GetById(int branchId, int goodCategoryId)
        {
            return FindBy(x => x.BranchId == branchId && x.GoodCategoryId == goodCategoryId).FirstOrDefault();
        }
    }
}
