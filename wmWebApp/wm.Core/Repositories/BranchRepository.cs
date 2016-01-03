using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Core.Models;
using System.Data.Entity;

namespace wm.Core.Repositories
{
    public interface IBranchRepository
    {
        bool Create(Branch obj);
        bool Delete(int id);
        Branch Details(int id);
        bool Edit(Branch obj);
        Branch GetById(int id);
        IEnumerable<Branch> GetList(string queryString);
        IEnumerable<Branch> GetListWithInclude(string queryString);
    }

    public class BranchRepository : IBranchRepository
    {
        private IntermediaryDbContext _context;
        public BranchRepository(IntermediaryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Branch> GetList(string queryString)
        {
            return _context.Branches.OrderBy(e => e.Name).ToList();
        }
        public IEnumerable<Branch> GetListWithInclude(string queryString)
        {
            return _context.Branches.Include(e => e.BranchType).OrderBy(e => e.Name).ToList();
        }

        public Branch GetById(int id)
        {
            return null;
        }

        public Branch Details(int id)
        {
            return null;
        }
        public bool Create(Branch obj)
        {
            return false;
        }

        public bool Edit(Branch obj)
        {
            return false;
        }

        public bool Delete(int id)
        {
            return false;
        }

        
    }
}
