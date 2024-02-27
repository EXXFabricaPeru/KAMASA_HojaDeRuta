using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Code
{
    public class Footer//: PdfPageEventHelper
    {

        //public override void OnEndPage(PdfWriter writer, Document doc)
        //{
        //    base.OnEndPage(writer, doc);
        //    iTextSharp.text.Font _miniFont = new iTextSharp.text.Font(
        //            iTextSharp.text.Font.FontFamily.HELVETICA, 10,
        //            iTextSharp.text.Font.NORMAL,
        //            BaseColor.BLACK);
        //    iTextSharp.text.Font _microFont = new iTextSharp.text.Font(
        //            iTextSharp.text.Font.FontFamily.HELVETICA, 5,
        //            iTextSharp.text.Font.NORMAL,
        //            BaseColor.BLACK);
        //    iTextSharp.text.Rectangle page = doc.PageSize;

        //    //Footer
        //    PdfPTable footer = new PdfPTable(1);
        //    footer.TotalWidth = page.Width - doc.LeftMargin - doc.RightMargin;
        //    //PdfPCell f1 = new PdfPCell(new Phrase("Estado de Cuenta : " +
        //    //        string.Format(DateTime.Now.ToShortDateString()),
        //    //        _miniFont));
        //    PdfPCell f1 = new PdfPCell(new Phrase(" ",
        //            _miniFont));
        //    f1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    f1.VerticalAlignment = Element.ALIGN_TOP;
        //    f1.HorizontalAlignment = Element.ALIGN_CENTER;
        //    f1.BorderWidthTop = 0.75f;

        //    footer.AddCell(f1);
        //    footer.WriteSelectedRows(
        //      0, -1,
        //      doc.LeftMargin,
        //      doc.BottomMargin - 10,
        //      writer.DirectContent
        //    );
        //}
        //public override void OnCloseDocument(PdfWriter writer, Document doc)
        //{
        //    base.OnCloseDocument(writer, doc);
        //}
    }
}
