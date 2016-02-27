using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IItemService : IEntityIntKeyService<Item>
    {
    }

    public class ItemService : EntityIntKeyService<Item>, IItemService
    {
        public ItemService(IUnitOfWork unitOfWork, IItemRepository Repos)
            : base(unitOfWork, Repos)
        {
        }

    }
}
