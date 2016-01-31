using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodCategoryGoodService : IEntityService<GoodCategoryGood>
    {
        IEnumerable<GoodCategoryGood> GetById(int goodId, int goodCategoryId);
        IEnumerable<GoodCategoryGood> GetByGoodCategoryId(int goodCategoryId, string include = "");
    }

    public class GoodCategoryGoodService : EntityService<GoodCategoryGood>, IGoodCategoryGoodService
    {
        IUnitOfWork _unitOfWork;
        IGoodCategoryGoodRepository _repos;

        public GoodCategoryGoodService(IUnitOfWork unitOfWork, IGoodCategoryGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public IEnumerable<GoodCategoryGood> GetByGoodCategoryId(int goodCategoryId, string include = "")
        {
            return _repos.Get((s => s.GoodCategoryId == goodCategoryId), null, include);
        }

        IEnumerable<GoodCategoryGood> IGoodCategoryGoodService.GetById(int goodId, int goodCategoryId)
        {
            return _repos.Get((s => s.GoodId == goodId && s.GoodCategoryId == goodCategoryId));
        }
    }
}
