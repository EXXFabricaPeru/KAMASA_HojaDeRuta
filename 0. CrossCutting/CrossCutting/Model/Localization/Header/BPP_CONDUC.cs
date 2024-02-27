using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class BPP_CONDUC : BaseSAPTable
    {
        public const string ID = "BPP_CONDUC";
        public const string DESCRIPTION = "Choferes";

        [EnhancedColumn(0), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_BPP_CHLI", "Licencia del conductor", BoDbTypes.Alpha)]
        public string License { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_BPP_CHNO", "Nombre del conductor", BoDbTypes.Alpha)]
        public string FullName { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_VS_TIPDOC", "Tipo Documento", BoDbTypes.Alpha)]
        public dynamic TipoDocumento { get; set; }
    }
}