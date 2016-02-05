using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IItemRepository : IGenericIntKeyRepository<Item>
    {
    }

    public class ItemRepository : GenericIntKeyRepository<Item>, IItemRepository
    {
        public ItemRepository(DbContext context)
            : base(context)
        {

        }

        //public override IEnumerable<Item> GetAll()
        //{
        //    return _entities.Set<Item>().AsEnumerable();//.Include(x => x.Country).AsEnumerable();
        //}
    }
}
