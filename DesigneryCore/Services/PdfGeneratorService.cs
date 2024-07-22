using DesigneryCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using Document = iText.Layout.Document;
using DesigneryCore.Interfaces;

namespace DesigneryCore.Services
{

public class PdfGeneratorService : IPdfGeneratorService
    {
        public byte[] GenerateOrderDetailsPdf()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Order Details"));
                document.Add(new Paragraph($"Order ID: "));
                document.Add(new Paragraph($"Customer Name: "));
                document.Add(new Paragraph($"Order Date: "));

                // Add more details as needed
                document.Close();
                return ms.ToArray();

                //return File(ms.ToArray(), "application/pdf", $"Order_orderId.pdf");
            }
        }
    }

}

