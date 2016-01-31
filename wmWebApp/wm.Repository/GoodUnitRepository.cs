using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodUnitRepository : IGenericRepository<GoodUnit>
    {
        GoodUnit GetById(int id);
    }

    class GoodUnitRepository : GenericRepository<GoodUnit>, IGoodUnitRepository
    {
        public GoodUnitRepository(DbContext context)
              : base(context)
        {

        }
        public GoodUnit GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
    }
}
