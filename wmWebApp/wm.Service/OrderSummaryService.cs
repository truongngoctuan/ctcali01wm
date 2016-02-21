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
    public interface IOrderSummaryService : IService
    {
        SummarizeMainKitchenOrderViewModel SummarizeMainKitchenOrder(IEnumerable<Order> orders, IEnumerable<Good> goodList, IEnumerable<Branch> branchList);
    }
    public class OrderSummaryService : IOrderSummaryService
    {
        //private IOrderService OrderService { get; set; }

        //IUnitOfWork _unitOfWork;
        //readonly IOrderRepository _repos;

        //public OrderSummaryService(OrderService orderService, IUnitOfWork unitOfWork, IOrderRepository repos)
        //{
        //    OrderService = orderService;
        //    _unitOfWork = unitOfWork;
        //    _repos = repos;
        //}

        public SummarizeMainKitchenOrderViewModel SummarizeMainKitchenOrder(IEnumerable<Order> orders, IEnumerable<Good> goodList, IEnumerable<Branch> branchList)
        {
            //var orders = _repos.Get((s => s.OrderDay == date));
            var flattenOrders = orders.SelectMany(s => s.OrderGoods
                    .Select(og => new
                    {
                        Branch = s.Branch,
                        OrderGood = og
                    })).ToList();

            var result = new List<SummarizeMainKitchenOrderItem>();
            foreach (var good in goodList)
            {
                var filteredFlattenOrders = flattenOrders.Where(s => s.OrderGood.GoodId == good.Id);
                var newData = new List<int>();
                foreach (var branch in branchList)
                {
                    var matches = filteredFlattenOrders.Where(s => s.Branch.Id == branch.Id);
                    newData.Add(matches.Any() ? matches.Sum(s => s.OrderGood.Quantity) : 0);
                }

                var newItem = new SummarizeMainKitchenOrderItem()
                {
                    Name = good.Name,
                    SummaryData = newData,
                    Total = newData.Sum()
                };
                result.Add(newItem);
            }

            return new SummarizeMainKitchenOrderViewModel()
            {
                Rows = result
            };
        }
    }


}
