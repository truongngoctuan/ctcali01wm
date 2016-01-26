using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{

    public interface IBranchService : IEntityService<Branch>
    {
        Branch GetById(int Id);
    }

    public class BranchService : EntityService<Branch>, IBranchService
    {
        IUnitOfWork _unitOfWork;
        IBranchRepository _repos;

        public BranchService(IUnitOfWork unitOfWork, IBranchRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public Branch GetById(int Id)
        {
            return _repos.GetById(Id);
        }
    }
}
