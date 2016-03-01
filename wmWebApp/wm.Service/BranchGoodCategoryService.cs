using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Service.Common;

namespace wm.Service
{
    public interface IBranchGoodCategoryService : IEntityService<BranchGoodCategory>
    {
        IEnumerable<BranchGoodCategory> GetByBranchId(int branchId, string include = "");
    }

    public class BranchGoodCategoryService : EntityService<BranchGoodCategory>, IBranchGoodCategoryService
    {

        public BranchGoodCategoryService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }
        
        public IEnumerable<BranchGoodCategory> GetByBranchId(int branchId, string include = "")
        {
            return Get((s => s.BranchId == branchId), (s => s.OrderBy(t => t.Ranking)), include);
        }

    }
}
