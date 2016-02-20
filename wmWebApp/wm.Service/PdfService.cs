﻿using System;
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
            BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
            Font NormalFont = new iTextSharp.text.Font(bf, 12, Font.NORMAL, BaseColor.BLACK);

            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            document.Add(new Paragraph("Hello World Trương Ngọc Tuấn", NormalFont));


            //Our sample HTML and CSS


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
            private static readonly string FontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
              "arialuni.ttf");

            private readonly BaseFont _baseFont;

            public UnicodeFontFactory()
            {
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