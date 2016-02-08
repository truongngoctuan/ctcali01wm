using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        IEnumerable<Employee> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered);

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


        public IEnumerable<Employee> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered)
        {
            var SortOrderSplit = SortOrder.Split(' ');

            var orderFunction = SortOrderSplit.Length == 2 ? _repos.GetOrderBy(SortOrderSplit[0], SortOrderSplit[1]) : _repos.GetOrderBy(SortOrderSplit[0]);

            recordsTotal = _repos.Get().Count();
            recordsFiltered = _repos.Get((s => SearchValue == null
            || s.Name.Contains(SearchValue)),
                orderFunction).Count();

            var resulFiltered = _repos.Get((s => SearchValue == null
            || s.Name.Contains(SearchValue)),
                orderFunction, "Branch", Start, Length);

            return resulFiltered;
        }

    }
}
