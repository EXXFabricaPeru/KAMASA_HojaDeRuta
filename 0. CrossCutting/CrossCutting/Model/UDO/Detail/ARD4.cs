using System;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class ARD4
    {
        public const string ID = "VS_PD_ARD4";
        public const string DESCRIPTION = "@VS_PD_OARD.ARD4";

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

        [EnhancedColumn, FieldNoRelated("U_EXK_FJOR", "Flujo Origen", BoDbTypes.Alpha)]
        public string FlowSource { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_ORSR", "Origen", BoDbTypes.Alpha, Size = 2)]
        [Val("OR", "Original")]
        [Val(DistributionDeliveryStatus.TRANSSHIPPING, DistributionDeliveryStatus.TRANSSHIPPING_DESCRIPTION)]
        public string OriginSource { get; set; }

        public string OriginSourceDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<ARD4, string>(t => t.OriginSource);
                return GenericHelper.GetDescriptionFromValue(property, OriginSource);
            }
        }
        
        [EnhancedColumn, FieldNoRelated("U_EXK_OTEN", "Código Orden Traslado", BoDbTypes.Integer)]
        public int ReferenceTransferEntry { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_OTNM", "Número Orden Traslado", BoDbTypes.Integer)]
        public int ReferenceTransferNumber { get; set; }
        
    }
}