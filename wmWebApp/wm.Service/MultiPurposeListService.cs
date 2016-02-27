using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IMultiPurposeListService : IEntityIntKeyService<MultiPurposeList>
    {
    }

    public class MultiPurposeListService : EntityIntKeyService<MultiPurposeList>, IMultiPurposeListService
    {
        public MultiPurposeListService(IUnitOfWork unitOfWork, IMultiPurposeListRepository Repos)
            : base(unitOfWork, Repos)
        {
        }

    }

    public interface IMultiPurposeListGoodService : IEntityService<MultiPurposeListGood>
    {
    }

    public class MultiPurposeListGoodService : EntityService<MultiPurposeListGood>, IMultiPurposeListGoodService
    {
        public MultiPurposeListGoodService(IUnitOfWork unitOfWork, IMultiPurposeListGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
        }

    }

    public interface IMultiPurposeListBranchService : IEntityService<MultiPurposeListBranch>
    {
    }

    public class MultiPurposeListBranchService : EntityService<MultiPurposeListBranch>, IMultiPurposeListBranchService
    {

        public MultiPurposeListBranchService(IUnitOfWork unitOfWork, IMultiPurposeListBranchRepository Repos)
            : base(unitOfWork, Repos)
        {
        }

    }
}
