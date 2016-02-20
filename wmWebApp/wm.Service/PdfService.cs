using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace wm.Service
{
    public interface IPdfService: IService
    {
        byte[] ConvertToPdf(string example_html, string example_css);
    }
    public class PdfService : IPdfService
    {
        public PdfService()
        {
            
        }
        //http://stackoverflow.com/questions/25164257/how-to-convert-html-to-pdf-using-itextsharp
        //how to use xmlBuilder

        //http://www.codeproject.com/Articles/66948/Rendering-PDF-views-in-ASP-MVC-using-iTextSharp
        //how to return pdf file

        //http://www.c-sharpcorner.com/UploadFile/f2e803/basic-pdf-creation-using-itextsharp-part-ii/
        //useful link
        
        public byte[] ConvertToPdf(string example_html, string example_css)
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            document.Add(new Paragraph("Hello World"));


            //Our sample HTML and CSS


            using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
            {
                using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                {

                    //Parse the HTML
                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml, msCss);
                }
            }
            document.Close();
            writer.Close();
            return ms.GetBuffer();
        }
    }
}
