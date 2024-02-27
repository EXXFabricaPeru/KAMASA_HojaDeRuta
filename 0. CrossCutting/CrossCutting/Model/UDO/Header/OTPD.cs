using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OTPD : BaseSAPTable
    {
        public const string ID = "VS_PD_OTPD";
        public const string DESCRIPTION = "Tabla Tiempo de Despacho";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Código")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_CLVT", "Canal de Venta", BoDbTypes.Alpha)]
        public string SaleChannel { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_TPDS", "Tiempo de Despacho", BoDbTypes.Integer)]
        public int DispatchTime { get; set; }
    }
}