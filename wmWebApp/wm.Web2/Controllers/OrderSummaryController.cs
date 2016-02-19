using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Service;

namespace wm.Web2.Controllers
{
    public class OrderSummaryController : Controller
    {
        private IPdfService PdfService { get; set; }

        public IOrderSummaryService OrderSummaryService { get; set; }
        public OrderSummaryController(PdfService pdfService, IOrderSummaryService orderSummaryService)
        {
            PdfService = pdfService;
            OrderSummaryService = orderSummaryService;
        }

        // GET: OrderSummary
        public ActionResult SummaryMainKitchenOrder(DateTime date, int branchId)
        {

            return View();
        }
    }
}