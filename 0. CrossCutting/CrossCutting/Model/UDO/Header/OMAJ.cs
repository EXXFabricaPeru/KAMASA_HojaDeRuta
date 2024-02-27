using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class OMAJ :BaseUDO
    {
        public const string ID = "VS_LT_OMAJ";
        public const string DESCRIPTION = "Motivos_Ajustes";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Código")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_VS_LT_CCAS", "Cuenta Asignada", BoDbTypes.Alpha, Size = 50)]
        public string CuentaAsignada { get; set; }

        //[EnhancedColumn(3), FieldNoRelated("U_VS_LT_CVAL", "Valor", BoDbTypes.Alpha, Size = 254)]
        //public string Value { get; set; }
    }
}
