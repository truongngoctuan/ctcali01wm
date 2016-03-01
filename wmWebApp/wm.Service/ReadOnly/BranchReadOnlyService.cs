using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Repository;
using wm.Repository.Shared;
using wm.Service.Common;

namespace wm.Service
{
    public interface IBranchReadOnlyService : IReadOnlyRepository<Branch>
    {
    }

    public class BranchReadOnlyService : ReadOnlyRepository<Branch>, IBranchReadOnlyService
    {
        public IReadOnlyRepository<Branch> ReadOnlyRepository { get; set; } 
        public BranchReadOnlyService(DbContext context)
            : base(context)
        {
            ReadOnlyRepository = new ReadOnlyRepository<Branch>(context);
        }
    }
}
