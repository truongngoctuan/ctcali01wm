using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodCategoryGoodService : IEntityService<GoodCategoryGood>
    {
        IEnumerable<GoodCategoryGood> GetByGoodCategoryId(int goodCategoryId, string include = "");
    }

    public class GoodCategoryGoodService : EntityService<GoodCategoryGood>, IGoodCategoryGoodService
    {
        public GoodCategoryGoodService(IUnitOfWork unitOfWork, DbContext context)
            : base(unitOfWork, context)
        {
        }

        public IEnumerable<GoodCategoryGood> GetByGoodCategoryId(int goodCategoryId, string include = "")
        {
            return Get((s => s.GoodCategoryId == goodCategoryId), (s => s.OrderBy(t => t.Ranking)), include);
        }
    }
}
