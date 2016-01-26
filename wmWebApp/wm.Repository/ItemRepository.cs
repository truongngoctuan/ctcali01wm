using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Item GetById(int id);
    }

    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(DbContext context)
            : base(context)
        {

        }

        public override IEnumerable<Item> GetAll()
        {
            return _entities.Set<Item>().AsEnumerable();//.Include(x => x.Country).AsEnumerable();
        }

        public Item GetById(int id)
        {
            //return _dbset.Include(x => x.Country).Where(x => x.Id == id).FirstOrDefault();
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
