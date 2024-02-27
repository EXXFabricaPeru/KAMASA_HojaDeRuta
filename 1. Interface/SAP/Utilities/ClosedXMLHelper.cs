using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class ClosedXMLHelper
    {
        public static IXLRow RetrieveRow(this IList<IXLRow> list, int rowNumber)
        {
            IXLRow row = list.FirstOrDefault(t=>t.RowNumber() == rowNumber);
            if (row == null)
                throw new Exception("[Error] Row number not found");

            return row;
        }
    }
}