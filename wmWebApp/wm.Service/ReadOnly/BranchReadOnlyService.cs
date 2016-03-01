using System.Data.Entity;
using wm.Model;

namespace wm.Service
{
    public interface IBranchReadOnlyService : IReadOnlyService<Branch>
    {
    }

    public class BranchReadOnlyService : ReadOnlyService<Branch>, IBranchReadOnlyService
    {
        public IReadOnlyService<Branch> ReadOnlyService { get; set; } 
        public BranchReadOnlyService(DbContext context)
            : base(context)
        {
            ReadOnlyService = new ReadOnlyService<Branch>(context);
        }
    }
}
