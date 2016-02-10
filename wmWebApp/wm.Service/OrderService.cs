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
    public interface IOrderService : IEntityIntKeyService<Order>
    {
        Order Create(int branchId, DateTime orderDate);
        IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId);
        void placingOrder(int orderId, IEnumerable<OrderBranchItem> items);

        //for dashboard
        IEnumerable<Order> GetAllOrdersInMonth(DateTime monthIndicator, int branchId = 0);

        void ChangeStatus(int id, OrderStatus status);
    }

    public class OrderService : EntityIntKeyService<Order>, IOrderService
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
            var allList = _orderGoodService.GetByOrderId(orderId);
            foreach(var item in items)
            {
                if (item.Quantity > 0)
                {
                    var matches = allList.Where(s => s.GoodId == item.goodId);
                    if (matches.Any())
                    {
                        var match = matches.First();
                        match.Quantity = item.Quantity;
                        match.Note = item.Note;
                        _orderGoodService.Update(match);
                    }
                    else
                    {
                        _orderGoodService.Create(new OrderGood
                        {
                            OrderId = orderId,
                            GoodId = item.goodId,
                            Quantity = item.Quantity,
                            Note = item.Note,
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }
            }

        }

        public void ChangeStatus(int id, OrderStatus status)
        {
            var order = _repos.GetById(id);
            order.Status = status;
            Update(order);
        }


        #region dashboard
        public IEnumerable<Order> GetAllOrdersInMonth(DateTime monthIndicator, int branchId = 0)
        {
            DateTime startOfMonth = new DateTime(monthIndicator.Year,
                                               monthIndicator.Month,
                                               1);
            DateTime endOfMonth = new DateTime(monthIndicator.Year,
                                               monthIndicator.Month,
                                               DateTime.DaysInMonth(monthIndicator.Year,
                                                                    monthIndicator.Month));
            if (branchId == 0)
            {
                return _repos.Get((s => startOfMonth <= s.OrderDay && s.OrderDay <= endOfMonth), null, "");
            }
            return _repos.Get((s => startOfMonth <= s.OrderDay && s.OrderDay <= endOfMonth && s.BranchId == branchId), null, "");
        }
        #endregion

    }
}
