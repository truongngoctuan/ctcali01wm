using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Service.Common;

namespace wm.Service
{
    public interface IGoodService : IEntityIntKeyService<Good>
    {
        IEnumerable<Good> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered);

    }

    public class GoodService : EntityIntKeyService<Good>, IGoodService
    {
        public GoodService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

        public IEnumerable<Good> ListDatatables(string SearchValue, string SortOrder, int Start, int Length, out int recordsTotal, out int recordsFiltered)
        {
            var SortOrderSplit = SortOrder.Split(' ');

            var orderFunction = SortOrderSplit.Length == 2 ? GetOrderBy(SortOrderSplit[0], SortOrderSplit[1]) : GetOrderBy(SortOrderSplit[0]);

            recordsTotal = GetAll().Count();
            recordsFiltered = Get((s => SearchValue == null
            || s.Name.Contains(SearchValue)
            || s.NameASCII.Contains(SearchValue)
            ),
                orderFunction).Count();

            var resulFiltered = Get((s => SearchValue == null 
            || s.Name.Contains(SearchValue)
            || s.NameASCII.Contains(SearchValue)), 
                orderFunction, Start, Length, "Unit");

            return resulFiltered;
        }
    }
}
