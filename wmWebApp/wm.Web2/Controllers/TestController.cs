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
            var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
            var example_css = @".headline{font-size:200%}";
            byte[] buf = PdfService.ConvertToPdf(example_html, example_css);
            return new BinaryContentResult(buf, "application / pdf");
        }
    }
}