// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue

using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OPDS : BaseUDO
    {
        public const string ID = "EXX_HOAS_OPDS";
        public const string DESCRIPTION = "Configuración";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Código")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXX_HOAS_CTIP", "Tipo", BoDbTypes.Alpha, Size = 2)]
        [Val("ST", "Alfabético"), Val("IN", "Entero"), Val("DE", "Decimal"), Val("DH", "Fecha y Hora"), Val("HR", "Hora")]
        public string ValueType { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXX_HOAS_CVAL", "Valor", BoDbTypes.Alpha, Size = 254)]
        public string Value { get; set; }

        public static class ValueTypes
        {
            public const string ALPHABETICAL = "ST";
            public const string INTEGER = "IN";
            public const string DECIMAL = "DE";
            public const string DATE_HOUR = "DH";
        }

        public static class Codes
        {
            public const string DBUSER = "DBUSER";
            public const string DBPASS = "DBPASS";
            public const string SERVER = "SERVER";


            public const string IP_SERVICE_LAYER = "IPS_SL";
            public const string STORES_EXCLUDES = "TIE_EX";
            public const string ACCOUNT_COMISION = "COM_CT";
            public const string ROUND_VALUE= "RED_CA";

            public const string CLOSING_HOUR = "CLS_HR";
            public const string CLOSING_STATUS = "CLS_ST";
            public const string MINIMUN_ACCEPTATION_RANGE = "RNG_AP";
            public const string MINIMUN_ACCEPTATION_WEIGHT = "WHT_AP";
            public const string MINIMUN_ACCEPTATION_AMOUNT = "MNT_AP";
            public const string DISAPPROVED_RANGE_CODE = "RNG_ID";
            public const string DISAPPROVED_WEIGHT_CODE = "WHT_ID";
            public const string DISAPPROVED_AMOUNT_CODE = "MNT_ID";
            public const string MANDATORY_CHARGE_TYPE = "CHG_TP";
            public const string ROUTES_DEFAULT_QUANTITY = "QTY_RT";
            public const string ROUTES_AVERAGE_WEIGHT = "WHT_AG";
            public const string REST_API_PATH = "DIS_RP";
            public const string ROUTING_WEB_PATH = "ROT_RP";
            public const string DISTRIBUTION_CHANNEL_WITHOUT_REFERENCE = "CHN_DS";
            public const string DISTRIBUTION_TERRITORY_WITHOUT_REFERENCE = "TRR_DS";
            public const string DISAPPROVED_CERTIFICATE_ASSIGN_CODE = "CAS_ID";
            public const string DISAPPROVED_DISPATCH_HOUR_ASSIGN_CODE = "CAH_ID";
            public const string DISAPPROVED_ADDRESS_ASSIGN_CODE = "CAD_ID";
            public const string UNABLE_ALTITUDE_ASSIGN_CODE = "CAL_ID";
            public const string UNABLE_LONGITUDE_ASSIGN_CODE = "COL_ID";
            public const string RETURN_WAREHOUSE = "WHS_RT";
            public const string TEMPORAL_WAREHOUSE = "WHS_TM";
            public const string PRINCIPAL_WAREHOUSE = "WHS_PR";
            public const string KEY_API_BEETRACK = "BTK_KY";
            public const string URL_API_BEETRACK = "BTK_UR";
            public const string ENABLE_API_BEETRACK = "BTK_EN";
            public const string CHARGE_TAX = "CHG_TX";
            public const string SALE_ROUTE_SERIES = "RTS_VN";
            public const string SERVICE_ROUTE_SERIES = "RTS_SR";
            public const string PURCHASE_ROUTE_SERIES = "RTS_CM";
            public const string TRANSFER_ROUTE_SERIES = "RTS_TR";
            public const string RETURN_ROUTE_SERIES = "RTS_DV";
            public const string INTERNAL_CLIENT_NAME = "CCL_ID";
            public const string TRANSPORTER_GENERIC = "TRA_GEN";
            public const string CRYSTAL_SHARED_PATH = "DIR_RP";

            public const string DEFAULT_DISTRIBUTION_TIME_TRANSFER= "DST_DF";
            public const string DEFAULT_DISTRIBUTION_WINDOW = "DSW_DF";
            public const string DEFAULT_DISTRIBUTION_TIME_RETURN = "DST_DE";
            public const string DEFAULT_DISTRIBUTION_TIME_PURCHASE = "DST_DC";

            public const string PRINTER_NAME = "IMP_ID";
            public const string PRINTER_MAT_NAME = "IMM_ID";

            public const string EMAIL_EXT = "EXT_USU";
            public const string EMAIL_EXT_PASS = "EXT_PSS";
            public const string EMAIL_PORT = "COD_POR";
            public const string EMAIL_SMPT = "COD_SMT";

            public const string WHS_PICK = "WHS_RC";

            public const string ALERT_USERS_SALE_ORDER_CASH_PAYMENT = "SOCP_AL";
            public const string DISAPPROVED_SALE_ORDER_EMAIL_FILEPATH = "DIR_DE";

            public const string SERIE_DELIVERY = "SER_DE";
            public const string SERIE_INVOICE = "SER_IN";

            public const string SEND_SUNAT = "SUT_ST";
            public const string SEND_SUNAT_GR = "SUT_GR";


        }
    }
}
