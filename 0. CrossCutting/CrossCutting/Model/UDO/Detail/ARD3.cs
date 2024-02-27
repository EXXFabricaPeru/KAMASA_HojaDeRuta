using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class ARD3
    {
        public const string ID = "VS_PD_ARD3";
        public const string DESCRIPTION = "Documento Liquidado";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDTR", "Código Orden de Traslado", BoDbTypes.Integer)]
        public int TransferOrderEntry { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_NMTR", "Número Orden de Traslado", BoDbTypes.Integer)]
        public int TransferOrderNumber { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCEN", "Código del Documento Marketing", BoDbTypes.Integer)]
        public int ReferenceDocumentEntry { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCNM", "Número del Documento Marketing", BoDbTypes.Integer)]
        public int ReferenceDocumentNumber { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OBJT", "Tipo de objeto", BoDbTypes.Integer, Size = 11)]
        public int ReferenceDocumentSAPType { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCLN", "Linea del Documento Marketing", BoDbTypes.Integer), SAPForceUpdate]
        public int ReferenceDocumentLineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDAR", "Código del Artículo", BoDbTypes.Alpha, Size = 50)]
        public string ArticleCode { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DSAR", "Descripción del Artículo", BoDbTypes.Alpha, Size = 100)]
        public string ArticleDescription { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCTC", "Cantidad Despachar del Documento", BoDbTypes.Price)]
        public decimal DispatchQuantity { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCCA", "Cantidad Solicitada del Lote", BoDbTypes.Price)]
        public decimal RequestQuantity { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCRC", "Cantidad Rechazada del Lote", BoDbTypes.Price)]
        public decimal RejectQuantity { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFLT", "Lote de Referencia", BoDbTypes.Alpha, Size = 254)]
        public string ReferenceBatch { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_MTRC", "Motivo Rechazo", BoDbTypes.Alpha, Size = 254)]
        public string RejectMotive { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CORC", "Comentario Rechazo", BoDbTypes.Alpha, Size = 254)]
        public string RejectComment { get; set; }


        public DateTime? BatchExpiration { get; set; }
    }
}