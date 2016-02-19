using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Service;
using wm.Web2.Controllers.ActionResults;

namespace wm.Web2.Controllers
{
    
    public class TestController : Controller
    {
        protected PdfService PdfService { get; set; }
        public TestController()
        {
            PdfService = new PdfService();
        }
        // GET: Test
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ToPdf()
        {
            byte[] buf = PdfService.ConvertToPdf();
            return new BinaryContentResult(buf, "application / pdf");
        }
    }
}