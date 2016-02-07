using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IEmployeeService : IEntityService<Employee>
    {
        Employee GetById(int Id);
        Employee GetByApplicationId(string Id);
    }

    public class EmployeeService : EntityService<Employee>, IEmployeeService
    {
        IUnitOfWork _unitOfWork;
        IEmployeeRepository _repos;

        public EmployeeService(IUnitOfWork unitOfWork, IEmployeeRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public Employee GetById(int Id)
        {
            return _repos.Get((s => s.Id == Id), null, "").First();
        }

        public Employee GetByApplicationId(string Id)
        {
            return _repos.GetByApplicationUserId(Id);
        }

        public override IEnumerable<Employee> GetAll(string include = "")
        {
            return _repos.Get((s => s.Role != EmployeeRole.SuperUser), null, include);
        }
    }
}
