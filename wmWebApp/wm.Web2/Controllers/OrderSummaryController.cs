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
        public IBranchService BranchService { get; set; }
        public IGoodService GoodService { get; set; }
        public OrderSummaryController(ApplicationUserManager userManager, IOrderSummaryService orderSummaryService, IOrderService orderService, IBranchService branchService, IGoodService goodService)
            : base(userManager)
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

            ViewBag.branches = branches.Select(s => new { Id = s.Id, Name = s.Name });
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
            DateTime date = DateTime.Now.Date;
            var orders = OrderService.Get((s => s.OrderDay == date));
            var branches = BranchService.GetAll();//TODO: filter
            var goods = GoodService.Get((s => s.GoodType == GoodType.KitChenGood)); //filter

            var result = OrderSummaryService.SummarizeMainKitchenOrder(orders, goods, branches);

            ViewBag.branches = branches;

            //var result_html = RenderViewToString4(this.ControllerContext, "SummaryMainKitchenOrderTemplate", result);
            var result_html = RenderActionResultToString(this.View(result));
            //var result_html = RenderViewToString(this, "SummaryMainKitchenOrderTemplate", null);
            var example_css = FileToString(Server.MapPath("~/Views/OrderSummary/SummaryMainKitchenOrderTemplate.css"));

            byte[] buf = PdfService.ConvertToPdf(result_html, example_css);
            return new BinaryContentResult(buf, "application / pdf");
        }

        public ActionResult SummaryMainKitchenOrderTemplate()
        {
            DateTime date = DateTime.Now.Date;
            var orders = OrderService.Get((s => s.OrderDay == date));
            var branches = BranchService.GetAll();//TODO: filter
            var goods = GoodService.Get((s => s.GoodType == GoodType.KitChenGood)); //filter

            var result = OrderSummaryService.SummarizeMainKitchenOrder(orders, goods, branches);

            ViewBag.branches = branches;
            return View(result);
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

        public virtual string RenderView(ViewContext viewContext)
        {
            var response = viewContext.HttpContext.Response;
            response.Flush();
            var oldFilter = response.Filter;
            Stream filter = null;
            try
            {
                filter = new MemoryStream();
                response.Filter = filter;
                viewContext.View.Render(viewContext, viewContext.HttpContext.Response.Output);
                response.Flush();
                filter.Position = 0;
                var reader = new StreamReader(filter, response.ContentEncoding);
                return reader.ReadToEnd();
            }
            finally
            {
                if (filter != null)
                {
                    filter.Dispose();
                }
                response.Filter = oldFilter;
            }
        }

        static string RenderViewToString4(ControllerContext context,
                                            string viewPath,
                                            object model = null,
                                            bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

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