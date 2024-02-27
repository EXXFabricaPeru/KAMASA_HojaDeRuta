using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OCHA :BaseUDO
    {
        public const string ID = "VS_LP_CANALVENTA";
        public const string DESCRIPTION = "Canal  de Venta";


        [FormColumn(0, Description = "Code"), ColumnProperty("Code"), FindColumn]
        public string Code { get; set; }

        [FormColumn(1, Description = "Name"), ColumnProperty("Name")]
        public string Name { get; set; }

    }
}
