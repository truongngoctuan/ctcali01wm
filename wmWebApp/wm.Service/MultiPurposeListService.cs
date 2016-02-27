using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IMultiPurposeListService : IEntityIntKeyService<MultiPurposeList>
    {
    }

    public class MultiPurposeListService : EntityIntKeyService<MultiPurposeList>, IMultiPurposeListService
    {
        IUnitOfWork _unitOfWork;
        IMultiPurposeListRepository _repos;

        public MultiPurposeListService(IUnitOfWork unitOfWork, IMultiPurposeListRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

    }

    public interface IMultiPurposeListGoodService : IEntityService<MultiPurposeListGood>
    {
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

    }

    public interface IMultiPurposeListBranchService : IEntityService<MultiPurposeListBranch>
    {
    }

    public class MultiPurposeListBranchService : EntityService<MultiPurposeListBranch>, IMultiPurposeListBranchService
    {
        IUnitOfWork _unitOfWork;
        readonly IMultiPurposeListBranchRepository _repos;

        public MultiPurposeListBranchService(IUnitOfWork unitOfWork, IMultiPurposeListBranchRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

    }
}
