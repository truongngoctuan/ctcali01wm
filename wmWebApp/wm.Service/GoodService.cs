using System.Collections.Generic;
using System.Linq;
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
        public GoodService(IUnitOfWork unitOfWork, IGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
        }

        public IEnumerable<Good> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered)
        {
            var SortOrderSplit = SortOrder.Split(' ');

            var orderFunction = SortOrderSplit.Length == 2 ? Repos.GetOrderBy(SortOrderSplit[0], SortOrderSplit[1]) : Repos.GetOrderBy(SortOrderSplit[0]);

            recordsTotal = Repos.GetAll().Count();
            recordsFiltered = Repos.Get((s => SearchValue == null
            || s.Name.Contains(SearchValue)
            || s.NameASCII.Contains(SearchValue)
            ),
                orderFunction).Count();

            var resulFiltered = Repos.Get((s => SearchValue == null 
            || s.Name.Contains(SearchValue)
            || s.NameASCII.Contains(SearchValue)), 
                orderFunction, Start, Length, "Unit");

            return resulFiltered;
        }
    }
}
