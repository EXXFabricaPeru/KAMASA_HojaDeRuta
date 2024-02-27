// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class ODCR : BaseUDO
    {
        public const string ID = "VS_PD_ODCR";
        public const string DESCRIPTION = "@VS_PD_ODCR: Contador diario";

        public ODCR()
        {
            UsedCounters = new List<DCR1>();
        }

        [FormColumn(Description = @"Código", Visible = false), ColumnProperty(@"DocEntry")]
        public int DocumentEntry { get; set; }

        [FormColumn(Description = @"Número"), ColumnProperty(@"DocNum"), FindColumn]
        public int DocumentNumber { get; set; }

        [FormColumn, FieldNoRelated(@"U_EXK_FECR", @"Fecha del Contador", BoDbTypes.Date), FindColumn]
        public DateTime DayCounter { get; set; }

        [ChildProperty(DCR1.ID)] public List<DCR1> UsedCounters { get; set; }
    }
}