using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodUnitService : IEntityIntKeyService<GoodUnit>
    {
    }

    public class GoodUnitService : EntityIntKeyService<GoodUnit>, IGoodUnitService
    {
        public GoodUnitService(IUnitOfWork unitOfWork, IGoodUnitRepository Repos)
            : base(unitOfWork, Repos)
        {
        }

    }
}
