using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using wm.Model;
using wm.Repository;
using wm.Service.Model;

namespace wm.Service
{
    public interface IOrderSummaryService : IService
    {
        SummarizeMainKitchenOrder_Array_ViewModel SummarizeMainKitchenOrder_Array(IEnumerable<Order> orders, IEnumerable<Good> goodList, IEnumerable<Branch> branchList);

        SummarizeMainKitchenOrder_Dictionary_ViewModel SummarizeMainKitchenOrder_Dictionary(IEnumerable<Order> orders,
            IEnumerable<Good> goodList, IEnumerable<Branch> branchList);
    }
    public class OrderSummaryService : IOrderSummaryService
    {
        public SummarizeMainKitchenOrder_Array_ViewModel SummarizeMainKitchenOrder_Array(IEnumerable<Order> orders, IEnumerable<Good> goodList, IEnumerable<Branch> branchList)
        {
            //var orders = Repos.Get((s => s.OrderDay == date));
            var flattenOrders = orders.SelectMany(s => s.OrderGoods
                    .Select(og => new
                    {
                        Branch = s.Branch,
                        OrderGood = og
                    })).ToList();

            var result = new List<SummarizeMainKitchenOrder_Array_Item>();
            foreach (var good in goodList)
            {
                var filteredFlattenOrders = flattenOrders.Where(s => s.OrderGood.GoodId == good.Id);
                var newData = new List<int>();
                foreach (var branch in branchList)
                {
                    var matches = filteredFlattenOrders.Where(s => s.Branch.Id == branch.Id);
                    newData.Add(matches.Any() ? matches.Sum(s => s.OrderGood.Quantity) : 0);
                }

                var newItem = new SummarizeMainKitchenOrder_Array_Item()
                {
                    Id = good.Id,
                    Name = good.Name,
                    SummaryData = newData,
                    Total = newData.Sum()
                };
                result.Add(newItem);
            }

            return new SummarizeMainKitchenOrder_Array_ViewModel()
            {
                Rows = result
            };
        }

        public SummarizeMainKitchenOrder_Dictionary_ViewModel SummarizeMainKitchenOrder_Dictionary(IEnumerable<Order> orders, IEnumerable<Good> goodList, IEnumerable<Branch> branchList)
        {
            //var orders = Repos.Get((s => s.OrderDay == date));
            var flattenOrders = orders.SelectMany(s => s.OrderGoods
                    .Select(og => new
                    {
                        Branch = s.Branch,
                        OrderGood = og
                    })).ToList();

            var result = new List<SummarizeMainKitchenOrder_Dictionary_Item>();
            foreach (var good in goodList)
            {
                var filteredFlattenOrders = flattenOrders.Where(s => s.OrderGood.GoodId == good.Id);
                var newData = new Dictionary<string, int>();
                foreach (var branch in branchList)
                {
                    var matches = filteredFlattenOrders.Where(s => s.Branch.Id == branch.Id);
                    newData.Add(branch.Id.ToString(), matches.Any() ? matches.Sum(s => s.OrderGood.Quantity) : 0);
                }

                var newItem = new SummarizeMainKitchenOrder_Dictionary_Item()
                {
                    Id = good.Id,
                    Name = good.Name,
                    SummaryData = newData,
                    Total = newData.Sum(s => s.Value)
                };
                result.Add(newItem);
            }

            return new SummarizeMainKitchenOrder_Dictionary_ViewModel()
            {
                Rows = result
            };
        }
    }


}
