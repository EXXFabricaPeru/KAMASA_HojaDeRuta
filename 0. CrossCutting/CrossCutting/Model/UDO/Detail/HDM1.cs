using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class HDM1 : BaseUDO
    {
        public const string ID = "VS_PD_HDM1";
        public const string DESCRIPTION = "Detalle Historico";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public int DocEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OTEN", "Código Orden de Traslado", BoDbTypes.Integer)]
        public int DocumentEntryTransferOrder { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OTNM", "Número Orden de Traslado", BoDbTypes.Integer)]
        public int DocumentNumberTransferOrder { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_STAD", "Estado de Entrega", BoDbTypes.Alpha, Size = 2)]
        [Val(DistributionDeliveryStatus.DELIVERED, DistributionDeliveryStatus.DELIVERED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.REJECTED, DistributionDeliveryStatus.REJECTED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RESCHEDULED, DistributionDeliveryStatus.RESCHEDULED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.NOT_DELIVERED, DistributionDeliveryStatus.NOT_DELIVERED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RETURNED, DistributionDeliveryStatus.RETURNED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RETURNED_OBSERVATION, DistributionDeliveryStatus.RETURNED_OBSERVATION_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.TRANSFERRED, DistributionDeliveryStatus.TRANSFERRED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.TRANSFERRED_OBSERVATION, DistributionDeliveryStatus.TRANSFERRED_OBSERVATION_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.ASSIGN, DistributionDeliveryStatus.ASSIGN_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RESCHEDULED_TRANS, DistributionDeliveryStatus.RESCHEDULED_TRANS_DESCRIPTION)]
        public string DeliveryStatus { get; set; }
    }
}