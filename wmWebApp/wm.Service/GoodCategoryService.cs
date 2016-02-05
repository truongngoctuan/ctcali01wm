using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodCategoryService : IEntityService<GoodCategory>
    {
        GoodCategory GetById(int Id);
    }

    public class GoodCategoryService : EntityService<GoodCategory>, IGoodCategoryService
    {
        IUnitOfWork _unitOfWork;
        IGenericIntKeyRepository<GoodCategory> _repos;

        public GoodCategoryService(IUnitOfWork unitOfWork, IGenericIntKeyRepository<GoodCategory> Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public GoodCategory GetById(int Id)
        {
            return _repos.GetById(Id);
        }
    }
}
