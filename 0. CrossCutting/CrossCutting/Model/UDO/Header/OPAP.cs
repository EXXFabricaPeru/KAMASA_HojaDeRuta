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
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OPAP : BaseUDO
    {
        public const string ID = "VS_LT_OPAP";
        public const string DESCRIPTION = "Pasarelas_de_Pago";


        [FormColumn(0, Description = "Código"), ColumnProperty("Code"), FindColumn]
        public string Code { get; set; }

        [FormColumn(1, Description = "Descripción"), ColumnProperty("Name"), FindColumn]
        public string Name { get; set; }

        [FormColumn(2), FieldNoRelated("U_VS_LT_MONE", "Moneda", BoDbTypes.Alpha, Size = 50), FindColumn]
        public string Moneda { get; set; }

        [FormColumn(3), FieldNoRelated("U_VS_LT_ACCT", "Cuenta", BoDbTypes.Alpha, Size = 100)]
        public string Cuenta { get; set; }

        [FormColumn(4), FieldNoRelated("U_VS_LT_CDSN", "Código Socio", BoDbTypes.Alpha, Size = 100)]
        public string CodigoSocio { get; set; }

    }
}
