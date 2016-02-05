using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IBranchService : IEntityIntKeyService<Branch>
    {
    }

    public class BranchService : EntityIntKeyService<Branch>, IBranchService
    {
        IUnitOfWork _unitOfWork;
        IBranchRepository _repos;

        public BranchService(IUnitOfWork unitOfWork, IBranchRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }
    }
}
