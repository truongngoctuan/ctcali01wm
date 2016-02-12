using System.Linq;
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
        readonly IBranchRepository _repos;

        public BranchService(IUnitOfWork unitOfWork, IBranchRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public override ServiceReturn Create(Branch entity)
        {
            //check constraints
            if (entity.BranchType == BranchType.MainKitchen)
            {
                var nMainKitchen = _repos.GetAsNoTracking((s => s.BranchType == BranchType.MainKitchen)).Count();
                if (nMainKitchen > 0)
                {
                    return ServiceReturn.Error("There is a main kitchen in the system, you can't have more than one");
                }
            }

            if (entity.BranchType == BranchType.MainWarehouse)
            {
                var nMainWarehouse = _repos.GetAsNoTracking((s => s.BranchType == BranchType.MainWarehouse)).Count();
                if (nMainWarehouse > 0)
                {
                    return ServiceReturn.Error("There is a main warehouse in the system, you can't have more than one");
                }
            }

            return base.Create(entity);
        }

        public override ServiceReturn Update(Branch entity)
        {
            //check constraints
            if (entity.BranchType == BranchType.MainKitchen)
            {
                var MainKitchen = _repos.GetAsNoTracking((s => s.BranchType == BranchType.MainKitchen)).FirstOrDefault();
                if (MainKitchen!= null && MainKitchen.Id != entity.Id)
                {
                    return ServiceReturn.Error("There is a main kitchen in the system, you can't have more than one");
                }
            }

            if (entity.BranchType == BranchType.MainWarehouse)
            {
                var MainWarehouse = _repos.GetAsNoTracking((s => s.BranchType == BranchType.MainWarehouse)).FirstOrDefault();
                if (MainWarehouse != null && MainWarehouse.Id != entity.Id)
                {
                    return ServiceReturn.Error("There is a main warehouse in the system, you can't have more than one");
                }
            }
            return base.Update(entity);
        }

    }
}
