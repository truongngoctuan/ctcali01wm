using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Service.Common;

namespace wm.Service
{
    public interface IEmployeeReadOnlyService : IEntityIntKeyService<Employee>
    {
        Employee GetByApplicationId(string Id);

    }

    public class EmployeeReadOnlyService : EntityIntKeyService<Employee>, IEmployeeReadOnlyService
    {
        public EmployeeReadOnlyService(UnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

        public Employee GetByApplicationId(string Id)
        {////http://stackoverflow.com/questions/23201907/asp-net-mvc-attaching-an-entity-of-type-modelname-failed-because-another-ent
            return _dbset.AsNoTracking().First(s => s.ApplicationUserId == Id);
        }

        
    }
}
