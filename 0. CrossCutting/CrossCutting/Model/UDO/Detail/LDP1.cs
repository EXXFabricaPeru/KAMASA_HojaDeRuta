using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OLDP.ID, 1)]
    public class LDP1 : BaseUDO
    {
        public const string ID = "VS_PD_LDP1";
        public const string DESCRIPTION = "@VS_PD_OLDP.LDP1";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public int DocEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDRT", "Código Ruta", BoDbTypes.Alpha, Size = 8)]
        public string RouteEntry { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_NMRT", "Número Ruta", BoDbTypes.Integer)]
        public int RouteNumber { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_LQTP", "Tipo Tarifario", BoDbTypes.Alpha, Size = 2)]
        [Val(TransporterTariffRateType.FLATT, TransporterTariffRateType.FLATT_DESCRIPTION)]
        [Val(TransporterTariffRateType.DEMAND, TransporterTariffRateType.DEMAND_DESCRIPTION)]
        public string LiquidationType { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_LQCD", "Código Tarifario", BoDbTypes.Alpha, Size = 254)]
        public string LiquidationCode { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFWH", "Peso Referenciado", BoDbTypes.Price)]
        public decimal LimitWeight { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTEN", "Puntos Entregados", BoDbTypes.Integer)]
        public int DeliveredPoints { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTNE", "Puntos No Entregados", BoDbTypes.Integer)]
        public int NotDeliveredPoints { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTOT", "Puntos Totales", BoDbTypes.Integer)]
        public int TotalPoints { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VTOT", "Valor Total", BoDbTypes.Price)]
        public decimal TotalValue { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DTOT", "Descuento Total", BoDbTypes.Price)]
        public decimal TotalDiscounts { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_ATOT", "Ajuste Total", BoDbTypes.Price)]
        public decimal TotalAdjustments { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_MTOT", "Monto Total", BoDbTypes.Price)]
        public decimal TotalAmount { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_STAD", "Estado Confirmación", BoDbTypes.Alpha, Size = 1, Default = "N")]
        [Val("Y", "Si"),Val("N", "No")]
        public bool Status { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_LQST", "Estado Liquidación", BoDbTypes.Alpha, Size = 2)]
        [Val("LQ", "Liquidado"), Val("NQ", "Exonerado")]
        public string LiquidationStatus { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_LQRD", @"Fecha de Liquidación Ruta", BoDbTypes.Date)]
        public DateTime? RouteLiquidationDate { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_GTTP", "Guías Transportista", BoDbTypes.Alpha, Size = 254)]
        public string TransporterDelivery { get; set; }
    }
}