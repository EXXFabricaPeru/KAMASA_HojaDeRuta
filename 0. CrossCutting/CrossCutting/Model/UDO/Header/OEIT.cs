using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION, BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class OEIT : BaseSAPTable
    {
        public const string ID = "VS_PD_OEIT";
        public const string DESCRIPTION = "Tabla Intemedia - EDI";

        [EnhancedColumn(0), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_TPCD", "Código Plantilla", BoDbTypes.Alpha, Size = 254)]
        public string TemplateCode { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_SPPR", "Propiedad SAP", BoDbTypes.Alpha, Size = 254)]
        public string SAPProperty { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXK_SPVL", "Valor SAP", BoDbTypes.Alpha, Size = 254)]
        public string SAPValue { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXK_EDVL", "Valor EDI", BoDbTypes.Alpha, Size = 254)]
        public string EDIValue { get; set; }
    }
}