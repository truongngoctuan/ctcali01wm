using System.Data.Entity;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodUnitService : IEntityIntKeyService<GoodUnit>
    {
    }

    public class GoodUnitService : EntityIntKeyService<GoodUnit>, IGoodUnitService
    {
        public GoodUnitService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

    }
}
