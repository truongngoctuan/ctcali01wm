using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Repository;
using wm.ServiceCRUD;

namespace wm.Service
{
    public interface IEmployeeCrudService : IEntityIntKeyCRUDService<Employee>
    {
        Employee GetByApplicationId(string Id);
    }

    public class EmployeeCrudService : EntityIntKeyCRUDService<Employee>, IEmployeeCrudService
    {
        public EmployeeCrudService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

        public Employee GetByApplicationId(string Id)
        {////http://stackoverflow.com/questions/23201907/asp-net-mvc-attaching-an-entity-of-type-modelname-failed-because-another-ent
            return _dbset.AsNoTracking().First(s => s.ApplicationUserId == Id);
        }

        public override IEnumerable<Employee> GetAll(string include = "")
        {
            return _dbset.IncludeProperties(include).Where(s => s.Role != EmployeeRole.SuperUser);
        }

    }
}
