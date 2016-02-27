using System.Collections.Generic;
using System.Linq;
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
        public EmployeeService(IUnitOfWork unitOfWork, IEmployeeRepository repos)
            : base(unitOfWork, repos)
        {
        }

        public Employee GetById(int Id)
        {
            return _repos.First((s => s.Id == Id));
        }

        public Employee GetByApplicationId(string Id)
        {////http://stackoverflow.com/questions/23201907/asp-net-mvc-attaching-an-entity-of-type-modelname-failed-because-another-ent
            return _repos.GetAsNoTracking((s => s.ApplicationUserId == Id)).First();
        }

        public override IEnumerable<Employee> GetAll(string include = "")
        {
            return _repos.Get((s => s.Role != EmployeeRole.SuperUser), include);
        }


        public IEnumerable<Employee> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered)
        {
            var SortOrderSplit = SortOrder.Split(' ');

            var orderFunction = SortOrderSplit.Length == 2 ? _repos.GetOrderBy(SortOrderSplit[0], SortOrderSplit[1]) : _repos.GetOrderBy(SortOrderSplit[0]);

            recordsTotal = _repos.GetAll().Count();
            recordsFiltered = _repos.Get((s => SearchValue == null
            || s.Name.Contains(SearchValue)),
                orderFunction).Count();

            var resulFiltered = _repos.Get((s => SearchValue == null
            || s.Name.Contains(SearchValue)),
                orderFunction, Start, Length, "Branch");

            return resulFiltered;
        }

    }
}
