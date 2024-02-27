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
    [UDOServices(FindServices.DefinedFields, CanDelete = true, CanManageSeries = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OHDM : BaseUDO
    {
        public const string ID = "VS_PD_OHDM";
        public const string DESCRIPTION = "Historico de Distribución";

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum"), FindColumn]
        public int DocumentNumber { get; set; }

        [FormColumn(2, Description = "Serie"), ColumnProperty("Series")]
        public int Series { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_DCEN", "Código de Documento", BoDbTypes.Integer)]
        public int DocumentEntryReference { get; set; }

        [FormColumn(4), FieldNoRelated("U_EXK_DCNM", "Número de Documento", BoDbTypes.Integer)]
        public int DocumentNumberReference { get; set; }

        [FormColumn(5), FieldNoRelated("U_EXK_DCTP", "Tipo de Documento", BoDbTypes.Integer)]
        public int SAPObjectTypeReference { get; set; }

        [ChildProperty(HDM1.ID)]
        public List<HDM1> HistoryDetails { get; set; }
    }
}