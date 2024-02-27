using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class VS_PD_MOTR : BaseSAPTable
    {
        public const string ID = "VS_PD_MOTR";
        public const string DESCRIPTION = "Motivo Traslado ";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Code")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Name")]
        public string Name { get; set; }

    }
}