using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Code
{
    public class HeaderFooter //: iTextSharp.text.pdf.PdfPageEventHelper
    {
        //int cont = 1;
        //public override void OnEndPage(PdfWriter writer, Document doc)
        //{
        //    base.OnEndPage(writer, doc);

        //    //var x= writer.;
        //    iTextSharp.text.Font _miniFont = new iTextSharp.text.Font(
        //            iTextSharp.text.Font.FontFamily.HELVETICA, 10,
        //            iTextSharp.text.Font.NORMAL,
        //            BaseColor.BLACK);
        //    iTextSharp.text.Font _microFont = new iTextSharp.text.Font(
        //            iTextSharp.text.Font.FontFamily.HELVETICA, 5,
        //            iTextSharp.text.Font.NORMAL,
        //            BaseColor.BLACK);
        //    iTextSharp.text.Rectangle page = doc.PageSize;


        //    PdfPTable header = new PdfPTable(1);

        //    header.TotalWidth = page.Width - doc.LeftMargin - doc.RightMargin;
        //    PdfPCell f12 = new PdfPCell(new Phrase("Página " + cont.ToString(), _miniFont));
        //    cont++;

        //    f12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    f12.VerticalAlignment = Element.ALIGN_TOP;
        //    f12.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    f12.BorderWidthBottom = 0.75f;
        //    float x = 0;
        //    header.AddCell(f12);
        //    header.WriteSelectedRows(0, -1, doc.LeftMargin, 580, writer.DirectContent);

        //}

        //public override void OnCloseDocument(PdfWriter writer, Document doc)
        //{
        //    base.OnCloseDocument(writer, doc);
        //}

    }
}
