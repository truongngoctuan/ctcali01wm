using System.Collections.Generic;
using System.Linq;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IMultiPurposeListGoodService : IEntityService<MultiPurposeListGood>
    {
        IEnumerable<MultiPurposeListGood> GetById(int goodId, int goodCategoryId);
        IEnumerable<MultiPurposeListGood> GetByGoodCategoryId(int goodCategoryId, string include = "");
    }

    public class MultiPurposeListGoodService : EntityService<MultiPurposeListGood>, IMultiPurposeListGoodService
    {
        IUnitOfWork _unitOfWork;
        readonly IMultiPurposeListGoodRepository _repos;

        public MultiPurposeListGoodService(IUnitOfWork unitOfWork, IMultiPurposeListGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public IEnumerable<MultiPurposeListGood> GetByGoodCategoryId(int goodCategoryId, string include = "")
        {
            return _repos.Get((s => s.MultiPurposeListId == goodCategoryId), (s => s.OrderBy(t => t.Ranking)), include);
        }

        IEnumerable<MultiPurposeListGood> IMultiPurposeListGoodService.GetById(int goodId, int goodCategoryId)
        {
            return _repos.Get((s => s.GoodId == goodId && s.MultiPurposeListId == goodCategoryId));
        }
    }
}
