using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Repository;
using wm.Service.Model;

namespace wm.Service
{
    public interface IOrderService : IEntityService<Order>
    {
        Order GetById(int Id);
        Order Create(int branchId, DateTime orderDate);
        IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId);
        void placingOrder(int orderId, IEnumerable<OrderBranchItem> items);
    }

    public class OrderService : EntityService<Order>, IOrderService
    {
        IUnitOfWork _unitOfWork;
        IOrderRepository _repos;
        IOrderGoodService _orderGoodService;
        IGoodService _goodService;
        IGoodCategoryGoodService _goodCategoryGoodService;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository Repos,
            IOrderGoodService OrderGoodService,
            IGoodCategoryGoodService GoodCategoryGoodService)
            : base(unitOfWork, Repos)
        {
            _unitOfWork = unitOfWork;
            _repos = Repos;
            _orderGoodService = OrderGoodService;
            _goodCategoryGoodService = GoodCategoryGoodService;
        }

        public Order GetById(int Id)
        {
            return _repos.GetById(Id);
        }

        public Order Create(int branchId, DateTime orderDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId)
        {
            var allList = _goodCategoryGoodService.GetByGoodCategoryId(goodCategoryId, "Good").Select(s => s.Good);
            var filteredList = _orderGoodService.GetByOrderId(orderId);//TODO: have some redunrant but have no way to optimize yet

            //binding data
            var returnData = new List<OrderBranchItem>();
            foreach (var item in allList)
            {
                var matches = filteredList.Where(t => t.GoodId == item.Id);
                if (matches.Any())
                {//return with data
                    returnData.Add(new OrderBranchItem
                    {
                        orderId = orderId,
                        goodId = item.Id,
                        Name = item.Name,
                        Quantity = matches.First().Quantity,
                        Note = matches.First().Note
                    });
                }
                else
                {//return with default data
                    returnData.Add(new OrderBranchItem
                    {
                        orderId = orderId,
                        goodId = item.Id,
                        Name = item.Name,
                        Quantity = 0,
                        Note = ""
                    });
                }
            }

            return returnData;
        }

        public void placingOrder(int orderId, IEnumerable<OrderBranchItem> items)
        {
            throw new NotImplementedException();
        }
    }
}
