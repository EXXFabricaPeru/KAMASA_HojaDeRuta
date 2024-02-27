using System;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class ARD2 : BaseUDO
    {
        public const string ID = "VS_PD_ARD2";
        public const string DESCRIPTION = "Documento Referenciada";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDTR", "Código Orden de Traslado", BoDbTypes.Integer)]
        public int TransferOrderDocEntry { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_NMTR", "Número Orden de Traslado", BoDbTypes.Integer)]
        public int TransferOrderNumber { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCEN", "Código del Documento Marketing", BoDbTypes.Integer)]
        public int ReferenceDocumentEntry { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DCNM", "Número del Documento Marketing", BoDbTypes.Integer)]
        public int ReferenceDocumentNumber { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFMC", "Código del Comprobante", BoDbTypes.Integer)]
        public int ReferenceDocumentEntryInvoice { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFMM", "Número del Comprobante", BoDbTypes.Integer)]
        public int ReferenceDocumentNumberInvoice { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFMD", "Serie/Correlativo del Comprobante", BoDbTypes.Alpha, Size = 100)]
        public string ReferenceNumberAtCardInvoice { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFDC", "Código de la Guía", BoDbTypes.Integer)]
        public int ReferenceDocumentEntryDelivery { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFDM", "Número de la Guía", BoDbTypes.Integer)]
        public int ReferenceDocumentNumberDelivery { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFDD", "Serie/Correlativo de la Guía", BoDbTypes.Alpha, Size = 100)]
        public string ReferenceNumberAtCardDelivery { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFQC", "Código de la Cotización", BoDbTypes.Integer)]
        public int ReferenceDocumentEntryQuotation { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFQM", "Número de la Cotización", BoDbTypes.Integer)]
        public int ReferenceDocumentNumberQuotation { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTOT", "Peso Total", BoDbTypes.Price)]
        public double TotalWeight { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VTOT", "Volumen Total", BoDbTypes.Price)]
        public double TotalVolume { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_STAD", "Estado de Entrega", BoDbTypes.Alpha, Size = 2, Default = DistributionDeliveryStatus.PROGRAMMED)]
        [Val(DistributionDeliveryStatus.DELIVERED, DistributionDeliveryStatus.DELIVERED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.REJECTED, DistributionDeliveryStatus.REJECTED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RESCHEDULED, DistributionDeliveryStatus.RESCHEDULED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.NOT_DELIVERED, DistributionDeliveryStatus.NOT_DELIVERED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.IN_PROGRESS, DistributionDeliveryStatus.IN_PROGRESS_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RETURNED, DistributionDeliveryStatus.RETURNED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RETURNED_OBSERVATION, DistributionDeliveryStatus.RETURNED_OBSERVATION_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.PROGRAMMED, DistributionDeliveryStatus.PROGRAMMED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.TRANSFERRED, DistributionDeliveryStatus.TRANSFERRED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.TRANSFERRED_OBSERVATION, DistributionDeliveryStatus.TRANSFERRED_OBSERVATION_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.ASSIGN, DistributionDeliveryStatus.ASSIGN_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.TRANSSHIPPING, DistributionDeliveryStatus.TRANSSHIPPING_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RESCHEDULED_TRANS, DistributionDeliveryStatus.RESCHEDULED_TRANS_DESCRIPTION)]
        public string DeliveryStatus { get; set; }

        public string DeliveryStatusDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<ARD2, string>(t => t.DeliveryStatus);
                return GenericHelper.GetDescriptionFromValue(property, DeliveryStatus);
            }
        }

        [EnhancedColumn, FieldNoRelated("U_EXK_CMNT", "Comentario de Entrega", BoDbTypes.Alpha, Size = 254)]
        public string DeliveryComments { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OBJT", "Tipo de objeto", BoDbTypes.Integer, Size = 11)]
        public int ReferenceDocumentSAPType { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CCAL", "Estado Cert. Calidad", BoDbTypes.Alpha, Size = 100)]
        public string CertCalidadStatus { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CMST", "Estado Comprobate Electrónico", BoDbTypes.Alpha, Size = 100)]
        public string CompElectStatus { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_GEST", "Estado Generación", BoDbTypes.Alpha, Size = 100)]
        public string GeneracionStatus { get; set; }
    }
}