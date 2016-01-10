using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls; //For SortBy method
//TODO: remove System.Web in next version
namespace wm.Core.CRUDOperators
{
    public abstract class DatatablesUtil<T> where T : class
    {
        protected IQueryable<T> _source;

        //temp data
        int paramDraw;
        int recordsTotal;
        int recordsFiltered;
        public IEnumerable<T> GetListFiltered(DTParameters param)
        {
            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            IQueryable<T> afterFilter = FilterResult(param.Search.Value, _source, columnSearch);
            IQueryable<T> afterSortedPaginated = afterFilter
                .SortBy<T>(param.SortOrder)
                .Skip(param.Start).Take(param.Length);

            paramDraw = param.Draw;
            recordsFiltered = afterFilter.Count();
            recordsTotal = _source.Count();

            //List<TViewModel> data = this.ConvertToViewModel().ToList();

            //DTResult<TViewModel> result = new DTResult<TViewModel>
            //{
            //    draw = param.Draw,
            //    data = data,
            //    recordsFiltered = afterFilter.Count(),
            //    recordsTotal = source.Count()
            //};

            return afterSortedPaginated.AsEnumerable();
        }

        public DTResult<TViewModel> GetListDTResult<TViewModel>(IEnumerable<TViewModel> data)
        {
            DTResult<TViewModel> result = new DTResult<TViewModel>
            {
                draw = paramDraw,
                data = data.ToList(),
                recordsFiltered = recordsFiltered,
                recordsTotal = recordsTotal
            };

            return result;
        }

        protected abstract IQueryable<T> FilterResult(string search, IQueryable<T> dtResult, List<string> columnFilters);
        //protected abstract IQueryable<TViewModel> ConvertToViewModel();
    }

    //public abstract class DatatablesResult<T> where T : class
    //{
    //    public DTResult<T> GetResult(DTParameters param,
    //        IQueryable<T> source)
    //    {
    //        List<String> columnSearch = new List<string>();

    //        foreach (var col in param.Columns)
    //        {
    //            columnSearch.Add(col.Search.Value);
    //        }

    //        IQueryable<T> afterFilter = FilterResult(param.Search.Value, source, columnSearch);
    //        List<T> data = afterFilter
    //            .SortBy<T>(param.SortOrder)
    //            .Skip(param.Start).Take(param.Length)
    //            .ToList();
    //        DTResult<T> result = new DTResult<T>
    //        {
    //            draw = param.Draw,
    //            data = data,
    //            recordsFiltered = afterFilter.Count(),
    //            recordsTotal = source.Count()
    //        };

    //        return result;
    //    }

    //    protected abstract IQueryable<T> FilterResult(string search, IQueryable<T> dtResult, List<string> columnFilters);
    //}

    //public class ResultSetDatatablesCustomer : DatatablesResult<Customer>
    //{
    //    protected override IQueryable<Customer> FilterResult(string search, IQueryable<Customer> dtResult, List<string> columnFilters)
    //    {
    //        return dtResult.Where(p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower()) || p.City != null && p.City.ToLower().Contains(search.ToLower())
    //            || p.Postal != null && p.Postal.ToLower().Contains(search.ToLower()) || p.Email != null && p.Email.ToLower().Contains(search.ToLower()) || p.Company != null && p.Company.ToLower().Contains(search.ToLower()) || p.Account != null && p.Account.ToLower().Contains(search.ToLower())
    //            || p.CreditCard != null && p.CreditCard.ToLower().Contains(search.ToLower())))
    //            && (columnFilters[0] == null || (p.Name != null && p.Name.ToLower().Contains(columnFilters[0].ToLower())))
    //            && (columnFilters[1] == null || (p.City != null && p.City.ToLower().Contains(columnFilters[1].ToLower())))
    //            && (columnFilters[2] == null || (p.Postal != null && p.Postal.ToLower().Contains(columnFilters[2].ToLower())))
    //            && (columnFilters[3] == null || (p.Email != null && p.Email.ToLower().Contains(columnFilters[3].ToLower())))
    //            && (columnFilters[4] == null || (p.Company != null && p.Company.ToLower().Contains(columnFilters[4].ToLower())))
    //            && (columnFilters[5] == null || (p.Account != null && p.Account.ToLower().Contains(columnFilters[5].ToLower())))
    //            && (columnFilters[6] == null || (p.CreditCard != null && p.CreditCard.ToLower().Contains(columnFilters[6].ToLower())))
    //            );
    //    }
    //}
}
