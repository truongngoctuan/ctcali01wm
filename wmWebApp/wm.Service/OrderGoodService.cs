using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IOrderGoodService : IEntityService<OrderGood>
    {
        IEnumerable<OrderGood> GetById(int branchId, int goodCategoryId);
        IEnumerable<OrderGood> GetByOrderId(int orderId, string include = "");
    }

    public class OrderGoodService : EntityService<OrderGood>, IOrderGoodService
    {
        IUnitOfWork _unitOfWork;
        IOrderGoodRepository _repos;

        public OrderGoodService(IUnitOfWork unitOfWork, IOrderGoodRepository Repos)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
        }

        public IEnumerable<OrderGood> GetByOrderId(int orderId, string include = "")
        {
            return _repos.Get((s => s.OrderId == orderId), null, include);
        }

        IEnumerable<OrderGood> IOrderGoodService.GetById(int orderId, int goodId)
        {
            return _repos.Get((s => s.OrderId == orderId && s.GoodId == goodId));
        }
    }
}
