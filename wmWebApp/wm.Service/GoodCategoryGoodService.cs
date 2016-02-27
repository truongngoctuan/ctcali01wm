using System.Collections.Generic;
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
        public GoodCategoryGoodService(IUnitOfWork unitOfWork, IGoodCategoryGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
        }

        public IEnumerable<GoodCategoryGood> GetByGoodCategoryId(int goodCategoryId, string include = "")
        {
            return _repos.Get((s => s.GoodCategoryId == goodCategoryId), (s => s.OrderBy(t => t.Ranking)), include);
        }
    }
}
