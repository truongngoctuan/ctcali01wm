using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IItemRepository : IGenericRepository<Item>
    {
    }

    public class ItemRepository : GenericRepository<Item>, IItemRepository
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
