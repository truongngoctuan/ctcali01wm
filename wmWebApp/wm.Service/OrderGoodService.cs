using System.Collections.Generic;
using System.Linq;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IOrderGoodService : IEntityService<OrderGood>
    {
        IEnumerable<OrderGood> GetByOrderId(int orderId, string include = "");
        IEnumerable<OrderGood> GetByOrderIdRange(IEnumerable<int> orderIds, GoodType? type = null);
    }

    public class OrderGoodService : EntityService<OrderGood>, IOrderGoodService
    {
        IUnitOfWork _unitOfWork;
        readonly IOrderGoodRepository _repos;

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

        public IEnumerable<OrderGood> GetByOrderIdRange(IEnumerable<int> orderIds, GoodType? type = null)
        {
            return _repos.GetByOrderIdRange(orderIds, type);
        }
    }
}
