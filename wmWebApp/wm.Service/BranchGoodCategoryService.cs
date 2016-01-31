using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IBranchGoodCategoryService : IEntityService<BranchGoodCategory>
    {
        IEnumerable<BranchGoodCategory> GetById(int branchId, int goodCategoryId);
        IEnumerable<BranchGoodCategory> GetByBranchId(int branchId, string include = "");
    }

    public class BranchGoodCategoryService : EntityService<BranchGoodCategory>, IBranchGoodCategoryService
    {
        IUnitOfWork _unitOfWork;
        IBranchGoodCategoryRepository _repos;

        public BranchGoodCategoryService(IUnitOfWork unitOfWork, IBranchGoodCategoryRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }
        
        public IEnumerable<BranchGoodCategory> GetByBranchId(int branchId, string include = "")
        {
            return _repos.Get((s => s.BranchId == branchId), null, include);
        }

        IEnumerable<BranchGoodCategory> IBranchGoodCategoryService.GetById(int branchId, int goodCategoryId)
        {
            return _repos.Get((s => s.BranchId == branchId && s.GoodCategoryId == goodCategoryId));
        }
    }
}
