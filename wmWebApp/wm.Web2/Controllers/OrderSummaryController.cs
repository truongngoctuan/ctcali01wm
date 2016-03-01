using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Service.Model;
using wm.Web2.Controllers.ActionResults;

namespace wm.Web2.Controllers
{
    public class OrderSummaryController : BaseController
    {
        private IPdfService PdfService { get; set; }

        public IOrderSummaryService OrderSummaryService { get; set; }
        public IOrderService OrderService { get; set; }
        public IBranchReadOnlyService BranchReadOnlyService { get; set; }
        public IGoodService GoodService { get; set; }
        public IMultiPurposeListService MultiPurposeListService { get; set; }
        public IMultiPurposeListGoodService MultiPurposeListGoodService { get; set; }
        public IMultiPurposeListBranchService MultiPurposeListBranchService { get; set; }
        public OrderSummaryController(ApplicationUserManager userManager, IOrderSummaryService orderSummaryService, IOrderService orderService, IBranchReadOnlyService branchReadOnlyService, IGoodService goodService, IMultiPurposeListService multiPurposeListService, IMultiPurposeListGoodService multiPurposeListGoodService, IMultiPurposeListBranchService multiPurposeListBranchService)
            : base(userManager)
        {
            PdfService = new PdfService();
            OrderSummaryService = orderSummaryService;
            OrderService = orderService;
            BranchReadOnlyService = branchReadOnlyService;
            GoodService = goodService;
            MultiPurposeListService = multiPurposeListService;
            MultiPurposeListGoodService = multiPurposeListGoodService;
            MultiPurposeListBranchService = multiPurposeListBranchService;
        }

        private IEnumerable<SelectListItem> GetSelectMultiPurposeList()
        {
            var list = MultiPurposeListService.GetAll();
            return list.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Name
            });
        }

        // GET: OrderSummary
        public ActionResult Index()
        {
            
            var multiPurposeItem = MultiPurposeListService.GetAll().First();
            var goods = MultiPurposeListGoodService.Get((s => s.MultiPurposeListId == multiPurposeItem.Id), (s => s.OrderBy(t => t.Ranking)), "Good").Select(s => s.Good);
            var branches = MultiPurposeListBranchService.Get((s => s.MultiPurposeListId == multiPurposeItem.Id), (s => s.OrderBy(t => t.Ranking)), "Branch").Select(s => s.Branch);
            ViewBag.branches = branches.Select(s => new { Id = s.Id, Name = s.Name });
            ViewBag.goods = goods.Select(s => new { Id = s.Id, Name = s.Name });

            var multiPurposeList = GetSelectMultiPurposeList();
            multiPurposeList.First().Selected = true;
            ViewBag.multiPurposeList = multiPurposeList;
            return View();
        }

        [HttpPost]
        public ActionResult SummaryMainKitchenOrder(DateTime date, int listId)
        {
            var orders = OrderService.Get((s => s.OrderDay == date), null, "Branch,OrderGoods");

            //use goods from a list
            var multiPurposeItem = MultiPurposeListService.GetById(listId);
            var goods = MultiPurposeListGoodService.Get((s => s.MultiPurposeListId == multiPurposeItem.Id),
                (s => s.OrderBy(t => t.Ranking)), "Good")
                .Select(s => s.Good).ToList();
            var branches = MultiPurposeListBranchService.Get((s => s.MultiPurposeListId == multiPurposeItem.Id), 
                (s => s.OrderBy(t => t.Ranking)), "Branch")
                .Select(s => s.Branch).ToList();

            var result = OrderSummaryService.SummarizeMainKitchenOrder_Array(orders, goods, branches);

            //convert to handsontable format
            var resultViewModel = new List<List<int>>();
            foreach (var item in result.Rows)
            {
                resultViewModel.Add(item.SummaryData.ToList());
            }
            return Json(resultViewModel);
        }

        public ActionResult SummaryMainKitchenOrderToPdf(DateTime date, int listId)
        {
            //DateTime date = DateTime.Now.Date;
            var orders = OrderService.Get((s => s.OrderDay == date), null, "Branch,OrderGoods");
            var multiPurposeItem = MultiPurposeListService.GetById(listId);
            var goods = MultiPurposeListGoodService.Get((s => s.MultiPurposeListId == multiPurposeItem.Id),
                (s => s.OrderBy(t => t.Ranking)), "Good")
                .Select(s => s.Good).ToList();
            var branches = MultiPurposeListBranchService.Get((s => s.MultiPurposeListId == multiPurposeItem.Id),
                (s => s.OrderBy(t => t.Ranking)), "Branch")
                .Select(s => s.Branch).ToList();

            var result = OrderSummaryService.SummarizeMainKitchenOrder_Array(orders, goods, branches);

            ViewBag.branches = branches;
            var result_html = RenderActionResultToString(this.View(result));
            var example_css = FileToString(Server.MapPath("~/Views/OrderSummary/SummaryMainKitchenOrderTemplate.css"));

            byte[] buf = PdfService.ConvertToPdf(result_html, example_css);
            return new BinaryContentResult(buf, "application / pdf");
        }

        public static string FileToString(string path)
        {
            string readContents;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }
            return readContents;
        }

        //now, only use for summary, so no need to move to abstract class
        protected string RenderActionResultToString(ActionResult result)
        {
            // Create memory writer.
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            // Create fake http context to render the view.
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request,
                fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                this.ControllerContext.RouteData,
                this.ControllerContext.Controller);
            var oldContext = System.Web.HttpContext.Current;
            System.Web.HttpContext.Current = fakeContext;

            // Render the view.
            result.ExecuteResult(fakeControllerContext);

            // Restore old context.
            System.Web.HttpContext.Current = oldContext;

            // Flush memory and return output.
            memWriter.Flush();
            return sb.ToString();
        }

    }
}