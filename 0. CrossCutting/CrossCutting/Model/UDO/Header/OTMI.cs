// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue

using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OTMI : BaseSAPTable
    {
        public const string ID = "VS_PD_OTMI";
        public const string DESCRIPTION = "Tabla de mappeo intermedia";

        [EnhancedColumn(0), ColumnProperty("DocumentEntry", ColumnName = "Código")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_SORC", "Entidad", BoDbTypes.Alpha, Size = 50)]
        public string Source { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXK_ORVL", "Valor origen", BoDbTypes.Alpha, Size = 254)]
        public string OriginValue { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXK_DSVL", "Valor destino", BoDbTypes.Alpha, Size = 254)]
        public string TargetValue { get; set; }

        public static class EntityType
        {
            public const string CORRELATIVE_SERIE_TYPE = "Serie";
            public const string DOCUMENT_DISTRIBUTION_STATUS = "Document_Distribution_Status";
            public const string TRANSFER_ORDER_STATUS = "Transfer_Order_Status";
            public const string DOCUMENT_DISTRIBUTION_FLOW_STATUS = "Document_Distribution_Flow_Status";
            public const string SAP_DOCUMENT_STATUS = "SAP_Document_Status";
            public const string DOCUMENT_CLOSING_HOUR = "Document_Closing_Hour";
            public const string DISTRIBUTION_ORIGIN_FLOW = "Origin_flow";
        }
    }
}