using System.Data.Entity;
using wm.Model;
using wm.Service.Common;

namespace wm.Service
{
    public interface IMultiPurposeListService : IEntityIntKeyService<MultiPurposeList>
    {
    }

    public class MultiPurposeListService : EntityIntKeyService<MultiPurposeList>, IMultiPurposeListService
    {
        public MultiPurposeListService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

    }

    public interface IMultiPurposeListGoodService : IEntityService<MultiPurposeListGood>
    {
    }

    public class MultiPurposeListGoodService : EntityService<MultiPurposeListGood>, IMultiPurposeListGoodService
    {
        public MultiPurposeListGoodService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

    }

    public interface IMultiPurposeListBranchService : IEntityService<MultiPurposeListBranch>
    {
    }

    public class MultiPurposeListBranchService : EntityService<MultiPurposeListBranch>, IMultiPurposeListBranchService
    {

        public MultiPurposeListBranchService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

    }
}
