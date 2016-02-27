using System.Data.Entity;
using wm.Model;

namespace wm.Repository
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
    }

    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(DbContext context)
              : base(context)
        {

        }
    }
}
