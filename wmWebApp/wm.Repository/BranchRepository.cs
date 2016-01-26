using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Branch GetById(int id);
    }

    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(DbContext context)
              : base(context)
        {

        }
        public Branch GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
    }
}
