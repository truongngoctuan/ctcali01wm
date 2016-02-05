using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodRepository : IGenericIntKeyRepository<Good>
    {
    }

    public class GoodRepository : GenericIntKeyRepository<Good>, IGoodRepository
    {
        public GoodRepository(DbContext context)
              : base(context)
        {

        }
    }
}
