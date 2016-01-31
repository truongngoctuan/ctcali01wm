using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IGoodService : IEntityService<Good>
    {
        Good GetById(int Id);
        IEnumerable<Good> GetAllInclude();
        //IEnumerable<Good> GetByGoodCategory(int categoryId);
    }

    public class GoodService : EntityService<Good>, IGoodService
    {
        IUnitOfWork _unitOfWork;
        IGoodRepository _repos;

        public GoodService(IUnitOfWork unitOfWork, IGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public Good GetById(int Id)
        {
            return _repos.GetById(Id);
        }
        public  IEnumerable<Good> GetAllInclude()
        {
            return _repos.Get(null, null, "Unit");
        }

        //public IEnumerable<Good> GetByGoodCategory(int categoryId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
