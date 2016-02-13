using System;
using System.Collections.Generic;
using System.Linq;
using wm.Model;
using wm.Repository;
using wm.Service.Model;

namespace wm.Service
{
    public interface IOrderService : IEntityIntKeyService<Order>
    {
        IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId);
        IEnumerable<OrderMainKitchenItem> PopulateMainKitchenData(int orderId, int goodCategoryId);
        void Place(int orderId, IEnumerable<OrderBranchItem> items);

        //for dashboard
        IEnumerable<Order> GetAllOrdersInMonth(DateTime monthIndicator, int branchId = 0, string include = "");

        void ChangeStatus(int id, OrderStatus status);
    }

    public class OrderService : EntityIntKeyService<Order>, IOrderService
    {
        IUnitOfWork _unitOfWork;
        readonly IOrderRepository _repos;
        readonly IOrderGoodService _orderGoodService;
        public IGoodService GoodService { get; }
        readonly IGoodCategoryGoodService _goodCategoryGoodService;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository repos,
            IOrderGoodService orderGoodService,
            IGoodCategoryGoodService goodCategoryGoodService)
            : base(unitOfWork, repos)
        {
            _unitOfWork = unitOfWork;
            _repos = repos;
            _orderGoodService = orderGoodService;
            _goodCategoryGoodService = goodCategoryGoodService;
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
                        OrderId = orderId,
                        GoodId = item.Id,
                        Name = item.Name,
                        Quantity = matches.First().Quantity,
                        Note = matches.First().Note
                    });
                }
                else
                {//return with default data
                    returnData.Add(new OrderBranchItem
                    {
                        OrderId = orderId,
                        GoodId = item.Id,
                        Name = item.Name,
                        Quantity = 0,
                        Note = ""
                    });
                }
            }

            return returnData;
        }

        public IEnumerable<OrderMainKitchenItem> PopulateMainKitchenData(int orderId, int goodCategoryId)
        {
            //TODO: filter with goodCategoryId too
            var order = GetById(orderId);
            var sameDateOrderIds = _repos.Get((s => s.OrderDay.Date == order.OrderDay.Date))
                .Select(t => t.Id);
            var orderGoodList = _orderGoodService.GetByOrderIdRange(sameDateOrderIds, GoodType.KitChenGood);
            var quantityBranchTotal = orderGoodList.GroupBy(
                s => s.Good.Id,
                s => s.Quantity,
                (key, g) => new
                {
                    GoodId = key,
                    QuantityTotal = g.Sum()
                }
                );

            var allList = _goodCategoryGoodService.GetByGoodCategoryId(goodCategoryId, "Good").Select(s => s.Good);
            var filteredList = _orderGoodService.GetByOrderId(orderId);//TODO: have some redunrant but have no way to optimize yet

            //get total data
            //var quantityBranchList = _repos.
            //binding data
            var returnData = new List<OrderMainKitchenItem>();
            foreach (var item in allList)
            {
                var quantityTotal = 0;
                if (item.GoodType == GoodType.KitChenGood)
                {
                    quantityTotal = quantityBranchTotal.First(s => s.GoodId == item.Id).QuantityTotal;
                }
                var matches = filteredList.Where(t => t.GoodId == item.Id);
                if (matches.Any())
                {//return with data
                    returnData.Add(new OrderMainKitchenItem
                    {
                        OrderId = orderId,
                        GoodId = item.Id,
                        Name = item.Name,
                        Quantity = matches.First().Quantity,
                        QuantityFromBranch = quantityTotal,
                        Note = matches.First().Note
                    });
                }
                else
                {//return with default data
                    returnData.Add(new OrderMainKitchenItem
                    {
                        OrderId = orderId,
                        GoodId = item.Id,
                        Name = item.Name,
                        Quantity = 0,
                        QuantityFromBranch = quantityTotal,
                        Note = ""
                    });
                }
            }

            return returnData;
        }

        public void Place(int orderId, IEnumerable<OrderBranchItem> items)
        {
            var allList = _orderGoodService.GetByOrderId(orderId);
            foreach(var item in items)
            {
                if (item.Quantity > 0)
                {
                    var matches = allList.Where(s => s.GoodId == item.GoodId);
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
                            GoodId = item.GoodId,
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
        public IEnumerable<Order> GetAllOrdersInMonth(DateTime monthIndicator, int branchId = 0, string include = "")
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
                return _repos.Get((s => startOfMonth <= s.OrderDay && s.OrderDay <= endOfMonth), null, include);
            }
            return _repos.Get((s => startOfMonth <= s.OrderDay && s.OrderDay <= endOfMonth && s.BranchId == branchId), null, include);
        }
        #endregion

    }


}
