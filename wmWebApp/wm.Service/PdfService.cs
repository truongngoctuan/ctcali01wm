using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using wm.Service.Common;

namespace wm.Service
{
    public interface IPdfService: IService
    {
        byte[] ConvertToPdf(string example_html, string example_css, string fontPath = "");
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
        
        public byte[] ConvertToPdf(string example_html, string example_css, string fontPath = "")
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            //document.Add(new Paragraph("Hello World Trương Ngọc Tuấn", NormalFont));

            using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
            {
                using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                {

                    //Parse the HTML
                    //http://stackoverflow.com/questions/10329863/display-unicode-characters-in-converting-html-to-pdf
                    //to display unicode, you need to change to proper font
                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml, msCss, Encoding.UTF8, new UnicodeFontFactory());
                }
            }
            document.Close();
            writer.Close();
            return ms.GetBuffer();
        }

        public class UnicodeFontFactory : FontFactoryImp
        {
            private static readonly string FontPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
              //System.Web.Hosting.HostingEnvironment.MapPath()
              //Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
              "Content/7-1523-ARIALUNI.ttf");

            private readonly BaseFont _baseFont;

            public UnicodeFontFactory()
            {
                //string FontPath = Path.Combine(fontPath, "7-1523-ARIALUNI.ttf");

                //Font arial = FontFactory.GetFont("Arial", );
                //_baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, false);
                //_baseFont = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
                _baseFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            }

            public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
              bool cached)
            {
                return new Font(_baseFont, size, style, color);
            }
        }
    }
}
