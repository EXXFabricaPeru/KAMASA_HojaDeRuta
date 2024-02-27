// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue

using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OVHR : BaseUDO
    {
        public const string ID = "VS_PD_OVHR";
        public const string DESCRIPTION = "Ventana_Horaria";

        [FormColumn(0, Description = "Code"), ColumnProperty("Code"),FindColumn]
        public string Code { get; set; }

        [FormColumn(1, Description = "Name"), ColumnProperty("Name"), FindColumn]
        public string Name { get; set; }

        [ChildProperty(VHR1.ID)]
        public List<VHR1> RelatedHours { get; set; }
    }
}