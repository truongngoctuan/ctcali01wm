using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Repository;

namespace wm.Service
{
    public interface IOrderSummaryService : IService
    {
        IEnumerable<List<string>> SummarizeMainKitchenOrder(DateTime date);
    }
    public class OrderSummaryService: IOrderSummaryService
    {
        private IOrderService OrderService { get; set; }

        IUnitOfWork _unitOfWork;
        readonly IOrderRepository _repos;

        public OrderSummaryService(OrderService orderService, IUnitOfWork unitOfWork, IOrderRepository repos)
        {
            OrderService = orderService;
            _unitOfWork = unitOfWork;
            _repos = repos;
        }

        public IEnumerable< List<string> > SummarizeMainKitchenOrder(DateTime date)
        {
            var orders = _repos.Get((s => s.OrderDay == date));

            throw new NotImplementedException();
        }
    }
}
