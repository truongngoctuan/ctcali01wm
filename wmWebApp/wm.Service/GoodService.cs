using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodService : IEntityIntKeyService<Good>
    {
        IEnumerable<Good> GetAllInclude();
    }

    public class GoodService : EntityIntKeyService<Good>, IGoodService
    {
        IUnitOfWork _unitOfWork;
        IGoodRepository _repos;

        public GoodService(IUnitOfWork unitOfWork, IGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public  IEnumerable<Good> GetAllInclude()
        {
            return _repos.Get(null, null, "Unit");
        }
    }
}
