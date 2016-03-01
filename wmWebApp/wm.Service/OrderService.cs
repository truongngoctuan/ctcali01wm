using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Service.Common;
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
        readonly IOrderGoodService _orderGoodService;
        public IGoodService GoodService { get; }
        readonly IGoodCategoryGoodService _goodCategoryGoodService;

        public IOrderSummaryService OrderSummaryService { get; set; }
        public IBranchReadOnlyService BranchReadOnlyService { get; set; }

        public OrderService(IUnitOfWork unitOfWork, DbContext context,
            IOrderGoodService orderGoodService,
            IGoodCategoryGoodService goodCategoryGoodService, IBranchReadOnlyService branchReadOnlyService)
            : base(unitOfWork, context)
        {
            UnitOfWork = unitOfWork;
            _orderGoodService = orderGoodService;
            _goodCategoryGoodService = goodCategoryGoodService;
            BranchReadOnlyService = branchReadOnlyService;
            OrderSummaryService = new OrderSummaryService();
        }

        public override ServiceReturn Create(Order entity)
        {
            //update order indexing
            var orders = Get((s => s.OrderDay == entity.OrderDay && s.BranchId == entity.BranchId));
            entity.Indexing = orders.Count();

            return base.Create(entity);
        }

        public IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId)
        {
            var allList = _goodCategoryGoodService.GetByGoodCategoryId(goodCategoryId, "Good").Select(s => s.Good).ToList();
            var filteredList = _orderGoodService.GetByOrderId(orderId).ToList();//TODO: have some redunrant but have no way to optimize yet

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
                        InStock = matches.First().InStock,
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
                        InStock = 0,
                        Quantity = 0,
                        Note = ""
                    });
                }
            }

            return returnData;
        }

        public IEnumerable<OrderMainKitchenItem> PopulateMainKitchenData(int orderId, int goodCategoryId)
        {
            var order = GetById(orderId);
            //get summary data from branches


            //TODO: filter with goodCategoryId too

            var sameDateOrders = Get((s => s.OrderDay == order.OrderDay));
            var sameDateOrderIds = sameDateOrders.Select(t => t.Id);
            var orderGoodList = _orderGoodService.GetByOrderIdRange(sameDateOrderIds, GoodType.KitChenGood);
            var quantityBranchTotal = orderGoodList.GroupBy(
                s => s.Good.Id,
                s => s.Quantity,
                (key, g) => new
                {
                    GoodId = key,
                    QuantityTotal = g.Sum()
                }
                ).ToList();

            var allList = _goodCategoryGoodService.GetByGoodCategoryId(goodCategoryId, "Good").Select(s => s.Good).ToList();
            var filteredList = _orderGoodService.GetByOrderId(orderId).AsEnumerable().ToList();//TODO: have some redunrant but have no way to optimize yet

            //get summary data from branches
            var summaryData = OrderSummaryService.SummarizeMainKitchenOrder_Dictionary(sameDateOrders, allList, BranchReadOnlyService.Get((s => s.BranchType != BranchType.MainKitchen)).ToList());

            //get total data
            //var quantityBranchList = Repos.
            //binding data
            var returnData = new List<OrderMainKitchenItem>();
            foreach (var item in allList)
            {
                var quantityTotal = 0;
                if (item.GoodType == GoodType.KitChenGood)
                {
                    var matchesGoodType = quantityBranchTotal.Where(s => s.GoodId == item.Id);
                    if (matchesGoodType.Any())
                    {
                        quantityTotal = matchesGoodType.First().QuantityTotal;
                    }
                    
                }
                var matches = filteredList.Where(t => t.GoodId == item.Id);
                if (matches.Any())
                {//return with data
                    returnData.Add(new OrderMainKitchenItem
                    {
                        OrderId = orderId,
                        GoodId = item.Id,
                        Name = item.Name,
                        InStock = matches.First().InStock,
                        Quantity = matches.First().Quantity,
                        QuantityFromBranch = quantityTotal,
                        Note = matches.First().Note,
                        Details = summaryData.Rows.First(s => s.Id == item.Id).SummaryData
                    });
                }
                else
                {//return with default data
                    returnData.Add(new OrderMainKitchenItem
                    {
                        OrderId = orderId,
                        GoodId = item.Id,
                        Name = item.Name,
                        InStock = 0,
                        Quantity = 0,
                        QuantityFromBranch = quantityTotal,
                        Note = "",
                        Details = summaryData.Rows.First(s => s.Id == item.Id).SummaryData
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
                if (item.Quantity > 0 || item.InStock > 0 || item.YourNote != string.Empty)
                {
                    //TODO: update history for Note
                    var matches = allList.Where(s => s.GoodId == item.GoodId);
                    if (matches.Any())
                    {
                        var match = matches.First();
                        match.InStock = item.InStock;
                        match.Quantity = item.Quantity;
                        match.Note += item.YourNote;
                        _orderGoodService.Update(match);
                    }
                    else
                    {
                        _orderGoodService.Create(new OrderGood
                        {
                            OrderId = orderId,
                            GoodId = item.GoodId,
                            InStock = item.InStock,
                            Quantity = item.Quantity,
                            Note = item.YourNote,
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }
            }

        }

        public void ChangeStatus(int id, OrderStatus status)
        {
            var order = GetById(id);
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
                return Get((s => startOfMonth <= s.OrderDay && s.OrderDay <= endOfMonth), include);
            }
            return Get((s => startOfMonth <= s.OrderDay && s.OrderDay <= endOfMonth && s.BranchId == branchId), include);
        }
        #endregion

    }


}
