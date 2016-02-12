using System.Data.Entity;
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
