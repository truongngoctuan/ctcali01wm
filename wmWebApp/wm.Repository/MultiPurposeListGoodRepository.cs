using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IMultiPurposeListGoodRepository : IGenericRepository<MultiPurposeListGood>
    {
        MultiPurposeListGood GetById(int branchId, int categoryId);
    }

    public class MultiPurposeListGoodRepository : GenericRepository<MultiPurposeListGood>, IMultiPurposeListGoodRepository
    {
        public MultiPurposeListGoodRepository(DbContext context)
              : base(context)
        {

        }
        public MultiPurposeListGood GetById(int goodId, int goodCategoryId)
        {
            return FindBy(x => x.GoodId == goodId && x.MultiPurposeListId == goodCategoryId).FirstOrDefault();
        }
    }
}
