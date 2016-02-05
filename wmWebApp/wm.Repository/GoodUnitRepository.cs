using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodUnitRepository : IGenericIntKeyRepository<GoodUnit>
    {
    }

    public class GoodUnitRepository : GenericIntKeyRepository<GoodUnit>, IGoodUnitRepository
    {
        public GoodUnitRepository(DbContext context)
              : base(context)
        {

        }
    }
}
