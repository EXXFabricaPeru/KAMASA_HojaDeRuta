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
    public class OCVL : BaseSAPTable
    {
        public const string ID = "VS_PD_OCVL";
        public const string DESCRIPTION = "Tabla de Velocidades";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Código")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_HRIN", "Hora Inicio", BoDbTypes.Integer)]
        public int StartHour { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_HRFN", "Hora Fin", BoDbTypes.Integer)]
        public string EndHour { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXK_VLPR", "Velocidad Promedio", BoDbTypes.Price)]
        public decimal Velocity { get; set; }
    }
}