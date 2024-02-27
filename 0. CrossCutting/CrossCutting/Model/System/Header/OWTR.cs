using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable(nameof(OWTR), FormType = "dummy")]
    [Serializable]
    public class OWTR : BaseSAPTable
    {
        [SAPColumn("DocEntry")] public int DocumentEntry { get; set; }

        [SAPColumn("DocNum")] public int DocumentNumber { get; set; }

        [SAPColumn("DocDate")] public DateTime DocumentDate { get; set; }

        [SAPColumn("DocDueDate")] public DateTime DueDate { get; set; }

        [SAPColumn("Filler")] public string FromWarehouse { get; set; }

        [SAPColumn("ToWhsCode")] public string ToWarehouse { get; set; }

        [SAPColumn("Comments")] public string Comments { get; set; }

        [SAPColumn("U_BPV_SERI", false)] public string SerieNumeroControl { get; set; }

        [SAPColumn("U_BPV_NCON2", false)] public string Correlativo { get; set; }

        [SAPColumn("U_EXK_ROTE", false)]
        [FieldNoRelated("U_EXK_ROTE", "Código Orden de Traslado", BoDbTypes.Integer, Size = 11)]
        public int ReferenceTransferOrderEntry { get; set; }

        [FieldNoRelated("U_EXK_OTRF", "Código Orden de Traslado Referencia", BoDbTypes.Integer, Size = 11)]
        public int ReferenceTransferOrderEntryDoc { get; set; }

        [SAPColumn("U_EXK_ROTN", false)]
        [FieldNoRelated("U_EXK_ROTN", "Número Orden de Traslado", BoDbTypes.Integer, Size = 11)]
        public int ReferenceTransferOrderNumber { get; set; }

        [FieldNoRelated("U_EXK_NMCR", "Serie-Correlativo", BoDbTypes.Alpha, Size = 254)]
        public string ReferenceNumber { get; set; }

        [SAPColumn(detailLine: true)] public IList<SAPRelatedDocument> RelatedDocuments { get; set; }

        [SAPColumn(detailLine: true)] public IList<WTR1> Lines { get; set; }
    }
}