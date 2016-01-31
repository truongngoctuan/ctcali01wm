using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodCategoryRepository : IGenericRepository<GoodCategory>
    {
        GoodCategory GetById(int id);
    }

    class GoodCategoryRepository : GenericRepository<GoodCategory>, IGoodCategoryRepository
    {
        public GoodCategoryRepository(DbContext context)
              : base(context)
        {

        }
        public GoodCategory GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
    }
}
