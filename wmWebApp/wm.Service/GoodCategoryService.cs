using System.Data.Entity;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodCategoryService : IEntityIntKeyService<GoodCategory>
    {
    }

    public class GoodCategoryService : EntityIntKeyService<GoodCategory>, IGoodCategoryService
    {
        public GoodCategoryService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

    }
}
