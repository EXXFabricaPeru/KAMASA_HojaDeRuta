using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using VSVersionControl.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail
{
    [SystemTable(nameof(WTR1), FormType = "dummy")]
    [Serializable]
    public class WTR1 : BaseSAPTable
    {
        [SAPColumn("LineNum")] public int LineNumber { get; set; }

        [SAPColumn("ItemCode")] public string ItemCode { get; set; }

        [SAPColumn("Quantity")] public decimal Quantity { get; set; }

        [SAPColumn("BaseEntry")] public int BaseDocumentEntry { get; set; }

        [SAPColumn("BaseLine")] public int BaseLineNumber { get; set; }

        [SAPColumn("BaseType")] public int BaseDocumentType { get; set; }

        [SAPColumn("FromWhsCod")] public string FromWarehouse { get; set; }

        [SAPColumn("WhsCode")] public string ToWarehouse { get; set; }

        [SAPColumn("U_tipoOpT12")] public string SUNATOperation { get; set; }

        [SAPColumn(detailLine: true)] public IList<SAPSelectedBatch> SelectedBatches { get; set; }
    }
}