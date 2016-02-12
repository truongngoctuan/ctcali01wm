using System;
using wm.Model;
using wm.Repository;

namespace wm.Service.OrderServiceHelper
{
    public interface IOrderGeneralService : IEntityIntKeyService<Order>
    {
        Order Create(int branchId, DateTime orderDate);
        void ChangeStatus(int id, OrderStatus status);
    }

    public class OrderGeneralService : EntityIntKeyService<Order>, IOrderGeneralService
    {
        IUnitOfWork _unitOfWork;
        readonly IOrderRepository _repos;
        IOrderGoodService _orderGoodService;
        IGoodService _goodService;
        IGoodCategoryGoodService _goodCategoryGoodService;

        public OrderGeneralService(IUnitOfWork unitOfWork, IOrderRepository Repos,
            IOrderGoodService OrderGoodService,
            IGoodCategoryGoodService GoodCategoryGoodService)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
            _orderGoodService = OrderGoodService;
            _goodCategoryGoodService = GoodCategoryGoodService;
        }

        public Order Create(int branchId, DateTime orderDate)
        {
            throw new NotImplementedException();
        }
        
        public void ChangeStatus(int id, OrderStatus status)
        {
            var order = _repos.GetById(id);
            order.Status = status;
            Update(order);
        }
        

    }
}
