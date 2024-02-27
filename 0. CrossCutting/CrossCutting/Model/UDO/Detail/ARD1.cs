using System;
using System.Linq.Expressions;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using System.Reflection;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class ARD1 : BaseUDO
    {
        public const string ID = "VS_PD_ARD1";
        public const string DESCRIPTION = "Orden de Traslado Referenciada";
        
        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDTR", "Código Orden de Traslado", BoDbTypes.Integer)]
        public int TransferOrderDocEntry { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_NMTR", "Número Orden de Traslado", BoDbTypes.Integer)]
        public int TransferOrderNumber { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_COCL", "Código del Cliente", BoDbTypes.Alpha, Size = 50)]
        public string ClientId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DSCL", "Razón Social del Cliente", BoDbTypes.Alpha, Size = 254)]
        public string ClientName { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_COEN", "Código de Dir. Entrega", BoDbTypes.Alpha, Size = 50)]
        public string AddressId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DREN", "Dir. de Entrega", BoDbTypes.Alpha, Size = 254)]
        public string AddressDescription { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_HRDS", "Hora de Distribución", BoDbTypes.Hour)]
        public DateTime? DeliveryHour { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTOT", "Peso Total", BoDbTypes.Price)]
        public double TotalWeight { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VTOT", "Volumen Total", BoDbTypes.Price)]
        public double TotalVolume { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_TPCR", "Tipo de Carga", BoDbTypes.Alpha)]
        public string LoadType { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_FJOR", "Flujo Origen", BoDbTypes.Alpha)]
        public string FlowSource { get; set; }
        
        [EnhancedColumn, FieldNoRelated("U_EXK_STAD", "Estado de Entrega", BoDbTypes.Alpha, Size = 2)]
        [Val("DE", "Entregado") , Val("RE", "Rechazado"), Val("PR", "Rechazado Parcial")]
        [Val(DistributionDeliveryStatus.NOT_DELIVERED, DistributionDeliveryStatus.NOT_DELIVERED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RESCHEDULED, DistributionDeliveryStatus.RESCHEDULED_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.RESCHEDULED_TRANS, DistributionDeliveryStatus.RESCHEDULED_TRANS_DESCRIPTION)]
        [Val(DistributionDeliveryStatus.TRANSSHIPPING, DistributionDeliveryStatus.TRANSSHIPPING_DESCRIPTION)]
        public string DeliveryStatus { get; set; }

        public string DeliveryStatusDescription
        {
            get
            {
                if (string.IsNullOrEmpty(DeliveryStatus))
                    return string.Empty;

                PropertyInfo property = GenericHelper.GetPropertyForExpression<ARD1, string>(t => t.DeliveryStatus);
                return GenericHelper.GetDescriptionFromValue(property, DeliveryStatus);
            }
        }

        [EnhancedColumn, FieldNoRelated("U_EXK_ORGN", "Origen del Trasbordo", BoDbTypes.Integer)]
        public int Origin { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_FNSH", "Destino del Trasbordo", BoDbTypes.Integer)]
        public int Finish { get; set; }
         
        public static class Status
        {
            public const string DELIVERED = "DE";
            public const string REJECTED = "RE";
            public const string PARTIAL_REJECTED = "PR";
            public const string RESCHEDULED = "RS";
            public const string PARTIAL_RESCHEDULED = "PS";
            public const string NOT_DELIVERED = "ND";
        }

        public static string RetrieveSAPName<T>(Expression<Func<ARD1, T>> expression)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(expression);
            var attribute = property.GetCustomAttribute<FieldNoRelated>();
            return attribute.sAliasID;
        }
    }

}