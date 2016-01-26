using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IItemService : IEntityService<Item>
    {
        Item GetById(int Id);
    }

    public class ItemService : EntityService<Item>, IItemService
    {
        IUnitOfWork _unitOfWork;
        IItemRepository _repos;

        public ItemService(IUnitOfWork unitOfWork, IItemRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public Item GetById(int Id)
        {
            return _repos.GetById(Id);
        }
    }
}
