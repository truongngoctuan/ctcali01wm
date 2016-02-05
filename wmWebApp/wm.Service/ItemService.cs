using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IItemService : IEntityIntKeyService<Item>
    {
    }

    public class ItemService : EntityIntKeyService<Item>, IItemService
    {
        IUnitOfWork _unitOfWork;
        IItemRepository _repos;

        public ItemService(IUnitOfWork unitOfWork, IItemRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

    }
}
