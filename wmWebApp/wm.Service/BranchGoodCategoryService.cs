using System.Collections.Generic;
using System.Linq;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IBranchGoodCategoryService : IEntityService<BranchGoodCategory>
    {
        IEnumerable<BranchGoodCategory> GetByBranchId(int branchId, string include = "");
    }

    public class BranchGoodCategoryService : EntityService<BranchGoodCategory>, IBranchGoodCategoryService
    {
        IUnitOfWork _unitOfWork;
        readonly IBranchGoodCategoryRepository _repos;

        public BranchGoodCategoryService(IUnitOfWork unitOfWork, IBranchGoodCategoryRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }
        
        public IEnumerable<BranchGoodCategory> GetByBranchId(int branchId, string include = "")
        {
            return _repos.Get((s => s.BranchId == branchId), (s => s.OrderBy(t => t.Ranking)), include);
        }

    }
}
