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
        public BranchService(IUnitOfWork unitOfWork, IBranchRepository repos)
            : base(unitOfWork, repos)
        {
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
                var mainKitchen = _repos.GetAsNoTracking((s => s.BranchType == BranchType.MainKitchen)).FirstOrDefault();
                if (mainKitchen!= null && mainKitchen.Id != entity.Id)
                {
                    return ServiceReturn.Error("There is a main kitchen in the system, you can't have more than one");
                }
            }

            if (entity.BranchType == BranchType.MainWarehouse)
            {
                var mainWarehouse = _repos.GetAsNoTracking((s => s.BranchType == BranchType.MainWarehouse)).FirstOrDefault();
                if (mainWarehouse != null && mainWarehouse.Id != entity.Id)
                {
                    return ServiceReturn.Error("There is a main warehouse in the system, you can't have more than one");
                }
            }
            return base.Update(entity);
        }

    }
}
