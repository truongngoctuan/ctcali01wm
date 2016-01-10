using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Core.CRUDOperators;
using wm.Core.Models;

namespace wm.Core.Repositories
{
    public interface IApplicationUserRepository
    {
        bool Create(ApplicationUser obj);
        bool Delete(int id);
        ApplicationUser Details(int id);
        bool Edit(ApplicationUser obj);
        ApplicationUser GetById(int id);
        IEnumerable<ApplicationUser> GetList(string queryString);
        IEnumerable<ApplicationUser> GetListWithInclude(string queryString);

        IQueryable<ApplicationUser> GetListQueryable();
    }

    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private IntermediaryDbContext _context;
        public ApplicationUserRepository(IntermediaryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetList(string queryString)
        {
            return _context.Users.OrderBy(e => e.FirstName).ToList();
        }
        public IEnumerable<ApplicationUser> GetListWithInclude(string queryString)
        {
            return _context.Users.OrderBy(e => e.FirstName).ToList();
        }

        public IQueryable<ApplicationUser> GetListQueryable()
        {
            return _context.Users;
        }

        public ApplicationUser GetById(int id)
        {
            return null;
        }

        public ApplicationUser Details(int id)
        {
            return null;
        }
        public bool Create(ApplicationUser obj)
        {
            return false;
        }

        public bool Edit(ApplicationUser obj)
        {
            return false;
        }

        public bool Delete(int id)
        {
            return false;
        }


    }


    public class DataTablesUtilApplicationUser : DatatablesUtil<ApplicationUser>
    {
        public DataTablesUtilApplicationUser(IApplicationUserRepository repository)
        {
            _source = repository.GetListQueryable();
        }
        protected override IQueryable<ApplicationUser> FilterResult(string search, IQueryable<ApplicationUser> dtResult, List<string> columnFilters)
        {
            return dtResult.Where(p => (search == null || (p.UserName != null && p.UserName.ToLower().Contains(search.ToLower())
            || p.LastName != null && p.LastName.ToLower().Contains(search.ToLower())
            || p.FirstName != null && p.FirstName.ToLower().Contains(search.ToLower())
                )
                ));
        }
    }
}
