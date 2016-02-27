using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IMultiPurposeListRepository : IGenericRepository<MultiPurposeList>
    {
    }

    public class MultiPurposeListRepository : GenericRepository<MultiPurposeList>, IMultiPurposeListRepository
    {
        public MultiPurposeListRepository(DbContext context)
              : base(context)
        {

        }
    }


    public interface IMultiPurposeListGoodRepository : IGenericRepository<MultiPurposeListGood>
    {
    }

    public class MultiPurposeListGoodRepository : GenericRepository<MultiPurposeListGood>, IMultiPurposeListGoodRepository
    {
        public MultiPurposeListGoodRepository(DbContext context)
              : base(context)
        {

        }
    }


    public interface IMultiPurposeListBranchRepository : IGenericRepository<MultiPurposeListBranch>
    {
    }

    public class MultiPurposeListBranchRepository : GenericRepository<MultiPurposeListBranch>, IMultiPurposeListBranchRepository
    {
        public MultiPurposeListBranchRepository(DbContext context)
              : base(context)
        {

        }
    }
}
