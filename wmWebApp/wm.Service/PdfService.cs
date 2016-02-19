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
    interface IPdfService
    {
        byte[] ConvertToPdf();
    }
    public class PdfService : IPdfService
    {
        public byte[] ConvertToPdf()
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            document.Add(new Paragraph("Hello World"));
            document.Close();
            writer.Close();
            return ms.GetBuffer();
        }
    }
}
