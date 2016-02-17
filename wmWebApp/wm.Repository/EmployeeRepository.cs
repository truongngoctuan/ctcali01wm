using System.Data.Entity;
using System.Linq;
using wm.Model;

namespace wm.Repository
{
    //because employee is special class
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Employee GetByApplicationUserId(string id);
    }

    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context)
              : base(context)
        {

        }
        public Employee GetByApplicationUserId(string id)
        {//http://stackoverflow.com/questions/23201907/asp-net-mvc-attaching-an-entity-of-type-modelname-failed-because-another-ent
            return _dbset.AsNoTracking().First(s => s.ApplicationUserId == id);
        }
    }
}
