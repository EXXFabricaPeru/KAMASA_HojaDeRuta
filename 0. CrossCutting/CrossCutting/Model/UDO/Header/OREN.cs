// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue

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
    public class OREN : BaseSAPTable
    {
        public const string ID = "VS_PD_OREN";
        public const string DESCRIPTION = "Restricciones_de_Entrega";

        [FormColumn(0, Description = "Código"), ColumnProperty("Code"), FindColumn]
        public string Code { get; set; }

        [FormColumn(1, Description = "Descripción"), ColumnProperty("Name")]
        public string Name { get; set; }

        [FormColumn(2), FieldNoRelated("U_EXK_CPDC", "Capacidad de Carga", BoDbTypes.Price)]
        public decimal LoadCapacity { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_VLDC", "Volumen de Carga", BoDbTypes.Price)]
        public decimal VolumeCapacity { get; set; }

        [FormColumn(4), FieldNoRelated("U_EXK_TPDS", "Tipo de Distribución", BoDbTypes.Alpha, Size = 3)]
        public string DistributionType { get; set; }
    }
}
