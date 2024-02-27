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
    public class RTR1 : BaseUDO
    {
        public const string ID = "VS_PD_RTR1";
        public const string DESCRIPTION = "Documento  Relacionado";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocumentEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("VisOrder")]
        public int VisOrder { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCEN", "Código del Documento", BoDbTypes.Integer)]
        public int DCEN { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCNM", "Número del Documento", BoDbTypes.Integer)]
        public int DCNM { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCCC", "Correlativo del  documento", BoDbTypes.Alpha, Size = 100)]
        public string DCCC { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCFE", "Fecha de Emisión", BoDbTypes.Date, Size = 10)]
        public DateTime? DCFE { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCST", "Estado SAP del Documento", BoDbTypes.Alpha, Size = 2)]
        public string DCST { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PRDS", "Prioridad de Despacho", BoDbTypes.Alpha, Size = 2)]
        public string PRDS { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_STAD", "Estado Distribución del Documento", BoDbTypes.Alpha, Size = 2)]
        public string STAD { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_STMT", "Motivo del Estado Distribución", BoDbTypes.Alpha, Size = 10)]
        public string STMT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_STDS", "Descripcion del Motivo", BoDbTypes.Alpha, Size = 254)]
        public string STDS { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CNAR", "Cantidad de Artículos - Solicitado", BoDbTypes.Price)]
        public decimal CNAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CPAR", "Cantidad de Artículos - Planificado", BoDbTypes.Price)]
        public decimal CPAR { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTOT", "Peso Total - Solicitado", BoDbTypes.Price)]
        public decimal PTOT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PPTO", "Peso Total - Planificado", BoDbTypes.Price)]
        public decimal PPTO { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VTOT", "Volumen Total", BoDbTypes.Price)]
        public decimal VTOT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_MTOT", "Monto Total - Solicitado", BoDbTypes.Price)]
        public decimal MTOT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_MPTO", "Monto Total - Planificado", BoDbTypes.Price)]
        public decimal MPTO { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OBJT", "Tipo de objeto", BoDbTypes.Integer, Size = 11)]
        public int OBJT { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_INVR", "Comprobante Relacionado", BoDbTypes.Alpha, Size = 100)]
        public string InvoiceReference { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_INVC", "Númeración Comprobante", BoDbTypes.Alpha, Size = 20)]
        public string InvoiceSeries { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DLVR", "Guia Relacionada", BoDbTypes.Alpha, Size = 100)]
        public string DeliveryReference { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DLVC", "Númeración Guia", BoDbTypes.Alpha, Size = 20)]
        public string DeliverySeries { get; set; }

        public bool InclusionStatus { get; set; }

        public string ObjectTypeDescription
        {
            get
            {
                switch (OBJT)
                {
                    case SAPConstanst.ObjectType.SALE_ORDER:
                        return @"Orden de Venta";
                    case SAPConstanst.ObjectType.INVENTORY_TRANSFER:
                        return @"Transferencia de Inventario";
                    case SAPConstanst.ObjectType.RETURN_ORDER:
                        return @"Orden de Devolución";
                    case SAPConstanst.ObjectType.PURCHASE_ORDER:
                        return @"Orden de Compra";
                    default:
                        return string.Empty;
                }
            }
        }

        public static class ValidationStatus
        {
            public const string APPROVED = "AP";
            public const string DISAPPROVED = "DI";
            public const string MANUAL_UPDATE = "ME";
        }
    }
}