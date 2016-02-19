using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Service;

namespace wm.Web2.Controllers
{
    public class BinaryContentResult : ActionResult
    {
        private string ContentType;
        private byte[] ContentBytes;

        public BinaryContentResult(byte[] contentBytes, string contentType)
        {
            this.ContentBytes = contentBytes;
            this.ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = this.ContentType;

            var stream = new MemoryStream(this.ContentBytes);
            stream.WriteTo(response.OutputStream);
            stream.Dispose();
        }
    }

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