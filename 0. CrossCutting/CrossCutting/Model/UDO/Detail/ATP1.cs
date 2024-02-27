using System;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    /// <summary>
    /// Transporter rate
    /// </summary>
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class ATP1
    {
        public const string ID = "VS_PD_ATP1";
        public const string DESCRIPTION = "@VS_PD_OLDP.ATP1";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocumentEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_CDTR", "Código de Tarifario", BoDbTypes.Alpha, Size = 254)]
        public string Code { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_TPTR", "Tipo de Tarifario", BoDbTypes.Alpha, Size = 2)]
        [Val(TransporterTariffRateType.FLATT, TransporterTariffRateType.FLATT_DESCRIPTION)]
        [Val(TransporterTariffRateType.DEMAND, TransporterTariffRateType.DEMAND_DESCRIPTION)]
        public string RateType { get; set; }

        public string RateTypeDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<ATP1, string>(t => t.RateType);
                return GenericHelper.GetDescriptionFromValue(property, RateType);
            }
        }

        [EnhancedColumn, FieldNoRelated("U_EXK_PTEN", "Puntos Entregados", BoDbTypes.Integer)]
        public int DeliveredPoints { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_RFWH", "Peso Referenciado", BoDbTypes.Price)]
        public decimal LimitWeight { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VTOT", "Valor Total", BoDbTypes.Price)]
        public decimal TotalValue { get; set; }
    }
}