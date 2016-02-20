using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Controllers.ActionResults;

namespace wm.Web2.Controllers
{
    public class OrderSummaryController : BaseController
    {
        private IPdfService PdfService { get; set; }

        public IOrderSummaryService OrderSummaryService { get; set; }
        public IOrderService OrderService { get; set; }
        public IBranchService BranchService { get; set; }
        public IGoodService GoodService { get; set; }
        public OrderSummaryController(ApplicationUserManager userManager, IOrderSummaryService orderSummaryService, IOrderService orderService, IBranchService branchService, IGoodService goodService)
            :base(userManager)
        {
            PdfService = new PdfService();
            OrderSummaryService = orderSummaryService;
            OrderService = orderService;
            BranchService = branchService;
            GoodService = goodService;
        }

        // GET: OrderSummary
        public ActionResult Index()
        {
            var branches = BranchService.GetAll();//TODO: filter
            var goods = GoodService.Get((s => s.GoodType == GoodType.KitChenGood)); //filter

            ViewBag.branches = branches.Select(s => new { Id = s.Id, Name = s.Name});
            ViewBag.goods = goods.Select(s => new { Id = s.Id, Name = s.Name });
            return View();
        }

        [HttpPost]
        public ActionResult SummaryMainKitchenOrder(DateTime date)
        {
            var orders = OrderService.Get((s => s.OrderDay == date));
            var branches = BranchService.GetAll();//TODO: filter
            var goods = GoodService.Get((s => s.GoodType == GoodType.KitChenGood)); //filter

            var result = OrderSummaryService.SummarizeMainKitchenOrder(orders, goods, branches);

            //convert to handsontable format
            var resultViewModel = new List<List<int>>();
            foreach (var item in result.Rows)
            {
                resultViewModel.Add(item.SummaryData.ToList());
            }
            return Json(resultViewModel);
        }

        public ActionResult SummaryMainKitchenOrderToPdf()
        {
            var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
            var example_css = @".headline{font-size:200%}";
            byte[] buf = PdfService.ConvertToPdf(example_html, example_css);
            return new BinaryContentResult(buf, "application / pdf");
        }
    }
}