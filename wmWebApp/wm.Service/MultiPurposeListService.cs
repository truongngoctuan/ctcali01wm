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
}
