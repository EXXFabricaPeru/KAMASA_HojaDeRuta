// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument

using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION, BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class ODSP : BaseSAPTable
    {
        public const string ID = "VS_PD_ODSP";
        public const string DESCRIPTION = "Priorización de tipo de carga";

        [EnhancedColumn(0), ColumnProperty("DocumentEntry", ColumnName = "Código")]
        public int Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_TPCR", "Tipo de carga", BoDbTypes.Alpha, Size = 2)]
        public string ItemLoadType { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_TPPR", "Prioridad de carga", BoDbTypes.Quantity)]
        public int LoadTypePriority { get; set; }
    }
}