using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodRepository : IGenericRepository<Good>
    {
        Good GetById(int id);
    }

    class GoodRepository : GenericRepository<Good>, IGoodRepository
    {
        public GoodRepository(DbContext context)
              : base(context)
        {

        }
        public Good GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
    }
}
