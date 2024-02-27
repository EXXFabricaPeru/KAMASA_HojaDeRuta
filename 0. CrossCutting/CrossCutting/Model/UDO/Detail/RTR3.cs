// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class RTR3 : BaseUDO
    {
        public const string ID = "VS_PD_RTR3";
        public const string DESCRIPTION = "Lote Relacionado";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("VisOrder")]
        public int VisOrder { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCEN", "Código del Documento", BoDbTypes.Integer)]
        public int DCEN { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCNM", "Número del Documento", BoDbTypes.Integer)]
        public int DCNM { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCLN", "Linea de detalle del Documento", BoDbTypes.Integer)]
        public int DCLN { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDAR", "Código del Artículo", BoDbTypes.Alpha, Size = 50)]
        public string CDAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDAL", "Código de Almacén", BoDbTypes.Alpha, Size = 10)]
        public string CDAL { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDLT", "Código de Lote", BoDbTypes.Alpha, Size = 50)]
        public string CDLT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_FCFA", "Fecha de fabricación", BoDbTypes.Date)]
        public DateTime? FCFA { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_FCEX", "Fecha de expiración", BoDbTypes.Date)]
        public DateTime? FCEX { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCCA", "Cantidad Solicitada del Documento", BoDbTypes.Price)]
        public decimal DCCA { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CADI", "Cantidad Disponible", BoDbTypes.Price)]
        public decimal CADI { get; set; }

        //ruben
        [EnhancedColumn, FieldNoRelated("U_EXK_CADE", "Cantidad Despachar", BoDbTypes.Price)]
        public decimal CADE { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OBJT", "Tipo de objeto", BoDbTypes.Integer)]
        public int OBJT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CMCA", "Comentario de motivo", BoDbTypes.Alpha, Size = 254)]
        public string CMCA { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PEND", "Pendiente", BoDbTypes.Alpha, Size = 10),]
        public string PEND { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_MOTV", "Motivo", BoDbTypes.Alpha, Size = 250),]
        public string MOTV { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_IMPR", "Impreso", BoDbTypes.Alpha, Size = 10),]
        public string IMPR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OBVS", "Observación", BoDbTypes.Alpha, Size = 150),]
        public string OBSV { get; set; }
    }
}
