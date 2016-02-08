using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodService : IEntityIntKeyService<Good>
    {
        IEnumerable<Good> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered);

    }

    public class GoodService : EntityIntKeyService<Good>, IGoodService
    {
        IUnitOfWork _unitOfWork;
        IGoodRepository _repos;

        public GoodService(IUnitOfWork unitOfWork, IGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public IEnumerable<Good> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered)
        {
            var SortOrderSplit = SortOrder.Split(' ');

            var orderFunction = SortOrderSplit.Length == 2 ? _repos.GetOrderBy(SortOrderSplit[0], SortOrderSplit[1]) : _repos.GetOrderBy(SortOrderSplit[0]);

            recordsTotal = _repos.Get().Count();
            recordsFiltered = _repos.Get((s => SearchValue == null
            || s.Name.Contains(SearchValue)),
                orderFunction).Count();

            var resulFiltered = _repos.Get((s => SearchValue == null 
            || s.Name.Contains(SearchValue)), 
                orderFunction, "Unit", Start, Length);

            return resulFiltered;
        }
    }
}
