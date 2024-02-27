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
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OPDG : BaseUDO
    {
        public const string ID = "VS_PD_OPDG";
        public const string DESCRIPTION = "Geolocalización";

        [FormColumn(0, Description = "Código"), ColumnProperty("Code")]
        public string Code { get; set; }

        [FormColumn(1, Description = "Descripción"), ColumnProperty("Name")]
        public string Name { get; set; }

        [ChildProperty(PDG1.ID)] public List<PDG1> Coordinates { get; set; }
    }
}