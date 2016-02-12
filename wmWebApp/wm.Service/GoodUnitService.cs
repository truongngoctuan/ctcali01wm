using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodUnitService : IEntityIntKeyService<GoodUnit>
    {
    }

    public class GoodUnitService : EntityIntKeyService<GoodUnit>, IGoodUnitService
    {
        IUnitOfWork _unitOfWork;
        IGoodUnitRepository _repos;

        public GoodUnitService(IUnitOfWork unitOfWork, IGoodUnitRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

    }
}
