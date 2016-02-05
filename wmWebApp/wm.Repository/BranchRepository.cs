using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IBranchRepository : IGenericIntKeyRepository<Branch>
    {
    }

    public class BranchRepository : GenericIntKeyRepository<Branch>, IBranchRepository
    {
        public BranchRepository(DbContext context)
              : base(context)
        {

        }
    }
}
