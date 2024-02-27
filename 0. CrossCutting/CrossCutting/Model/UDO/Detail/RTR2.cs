// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument

using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class RTR2 : BaseUDO
    {
        public const string ID = "VS_PD_RTR2";
        public const string DESCRIPTION = "Entrega Relacionada";

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

        [EnhancedColumn, FieldNoRelated("U_EXK_DSAR", "Descripción del Artículo", BoDbTypes.Alpha, Size = 100)]
        public string DSAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_TVAR", "Tiempo de vida útil mínimo", BoDbTypes.Integer)]
        public int TVAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_TPCR", "Tipo de Carga", BoDbTypes.Alpha, Size = 2)]
        public string TPCR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDUM", "Código de Unidad de Medida", BoDbTypes.Alpha, Size = 100)]
        public string CDUM { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DSUM", "Descripción de Unidad de Medida", BoDbTypes.Alpha, Size = 100)]
        public string DSUM { get; set; }

        //ruben
        [EnhancedColumn, FieldNoRelated("U_EXK_DCCA", "Cantidad Planificada del Documento", BoDbTypes.Price)]
        public decimal DCCA { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCCP", "Cantidad Solicitada del Documento", BoDbTypes.Price)]
        public decimal DCCP { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PEAR", "Peso del Artículo", BoDbTypes.Price)]
        public decimal PEAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PSAR", "Peso Total del Artículo - Solicitado", BoDbTypes.Price)]
        public decimal PSAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PSAA", "Peso Total del Artículo - Planificado", BoDbTypes.Price)]
        public decimal PSAA { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VLAR", "Volumen del Artículo", BoDbTypes.Price)]
        public decimal VLAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VTAR", "Volumen Total del Artículo", BoDbTypes.Price)]
        public decimal VTAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PCAR", "Precio del Artículo", BoDbTypes.Price)]
        public decimal PCAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTAR", "Precio Total del Artículo - Solicitado", BoDbTypes.Price)]
        public decimal PTAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTAA", "Precio Total del Artículo - Planificado", BoDbTypes.Price)]
        public decimal PTAA { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OBJT", "Tipo de objeto", BoDbTypes.Integer)]
        public int OBJT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CAPR", "Cantidad Solicitada Previa", BoDbTypes.Price)]
        public decimal CAPR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CMCA", "Comentario de modificación", BoDbTypes.Alpha, Size = 254)]
        public string CMCA { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDAL", "Código de Almacén", BoDbTypes.Alpha, Size = 10),]
        public string CDAL { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PEND", "Pendiente", BoDbTypes.Alpha, Size = 10),]
        public string PEND { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_MOTV", "Motivo", BoDbTypes.Alpha, Size = 250),]
        public string MOTV { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_IMPR", "Impreso", BoDbTypes.Alpha, Size = 10),]
        public string IMPR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OBVS", "Observación", BoDbTypes.Alpha, Size = 150),]
        public string OBSV { get; set; }


        public string MotivoDefecto
        {
            get { return string.IsNullOrEmpty(MOTV)?"NO": MOTV; } set { }
        }
    }
}