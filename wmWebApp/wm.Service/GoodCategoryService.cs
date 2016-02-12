using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodCategoryService : IEntityIntKeyService<GoodCategory>
    {
    }

    public class GoodCategoryService : EntityIntKeyService<GoodCategory>, IGoodCategoryService
    {
        IUnitOfWork _unitOfWork;
        IGoodCategoryRepository _repos;

        public GoodCategoryService(IUnitOfWork unitOfWork, IGoodCategoryRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

    }
}
