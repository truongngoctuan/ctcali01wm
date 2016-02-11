using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;
using wm.Service.Model;

namespace wm.Service
{
    public interface IOrder2Service
    {
        Order Create(int branchId, DateTime orderDate);
        IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId);
        void MakeOrder(int orderId, IEnumerable<OrderBranchItem> items);
        IEnumerable<Order> GetAllOrdersInMonth(DateTime monthIndicator, int branchId = 0);
        void ChangeStatus(int id, OrderStatus status);
    }

    class Order2Service : IOrder2Service
    {
        public void ChangeStatus(int id, OrderStatus status)
        {
            throw new NotImplementedException();
        }

        public Order Create(int branchId, DateTime orderDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAllOrdersInMonth(DateTime monthIndicator, int branchId = 0)
        {
            throw new NotImplementedException();
        }

        public void MakeOrder(int orderId, IEnumerable<OrderBranchItem> items)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderBranchItem> PopulateData(int orderId, int goodCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
