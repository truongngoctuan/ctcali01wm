﻿using System.Data.Entity;
using wm.Model;
using wm.Service.Common;

namespace wm.Service
{
    public interface IItemService : IEntityIntKeyService<Item>
    {
    }

    public class ItemService : EntityIntKeyService<Item>, IItemService
    {
        public ItemService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

    }
}
