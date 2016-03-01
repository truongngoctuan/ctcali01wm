using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Service.Common;
using wm.ServiceCRUD.Shared;

namespace wm.ServiceCRUD
{
    public interface IBranchCrudService : IEntityIntKeyCRUDService<Branch>
    {
    }

    public class BranchCrudService : EntityIntKeyCRUDService<Branch>, IBranchCrudService
    {
        public BranchCrudService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

        public override ServiceReturn Create(Branch entity)
        {
            //check constraints
            switch (entity.BranchType)
            {
                case BranchType.MainKitchen:
                    var nMainKitchen = _dbset.AsNoTracking().Count(s => s.BranchType == BranchType.MainKitchen);
                    if (nMainKitchen > 0)
                    {
                        return ServiceReturn.Error("There is a main kitchen in the system, you can't have more than one");
                    }
                    break;
                case BranchType.MainWarehouse:
                    var nMainWarehouse = _dbset.AsNoTracking().Count(s => s.BranchType == BranchType.MainWarehouse);
                    if (nMainWarehouse > 0)
                    {
                        return ServiceReturn.Error("There is a main warehouse in the system, you can't have more than one");
                    }
                    break;
            }

            return base.Create(entity);
        }

        public override ServiceReturn Update(Branch entity)
        {
            //check constraints
            switch (entity.BranchType)
            {
                case BranchType.MainKitchen:
                    var mainKitchen = _dbset.AsNoTracking().First(s => s.BranchType == BranchType.MainKitchen);
                    if (mainKitchen!= null && mainKitchen.Id != entity.Id)
                    {
                        return ServiceReturn.Error("There is a main kitchen in the system, you can't have more than one");
                    }
                    break;
                case BranchType.MainWarehouse:
                    var mainWarehouse = _dbset.AsNoTracking().First(s => s.BranchType == BranchType.MainWarehouse);
                    if (mainWarehouse != null && mainWarehouse.Id != entity.Id)
                    {
                        return ServiceReturn.Error("There is a main warehouse in the system, you can't have more than one");
                    }
                    break;
            }
            return base.Update(entity);
        }
    }
}
