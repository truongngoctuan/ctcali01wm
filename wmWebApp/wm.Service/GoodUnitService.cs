using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodUnitService : IEntityService<GoodUnit>
    {
        GoodUnit GetById(int Id);
    }

    public class GoodUnitService : EntityService<GoodUnit>, IGoodUnitService
    {
        IUnitOfWork _unitOfWork;
        IGenericIntKeyRepository<GoodUnit> _repos;

        public GoodUnitService(IUnitOfWork unitOfWork, IGenericIntKeyRepository<GoodUnit> Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public GoodUnit GetById(int Id)
        {
            return _repos.GetById(Id);
        }
    }
}
