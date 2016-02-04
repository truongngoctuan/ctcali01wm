using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Employee GetById(string id);
    }

    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context)
              : base(context)
        {

        }
        public Employee GetById(string id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
    }
}
