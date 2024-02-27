using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false, CanCancel = false)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OFUP : BaseUDO
    {
        public const string ID = "VS_PD_OFUP";
        public const string DESCRIPTION = "Carga de archivo";

        public OFUP()
        {
            ColumnDetail = new List<FUP1>();
        }

        [FormColumn(0, Description = @"Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = @"Número"), ColumnProperty("DocNum"), FindColumn]
        public int DocumentNumber { get; set; }

        [FormColumn(2), FieldNoRelated(@"U_EXK_CDSN", @"Codigo SN", BoDbTypes.Alpha, Size = 50)]
        public string TemplateCode { get; set; }

        [FormColumn(3), FieldNoRelated(@"U_EXK_FILE", @"Archivo de carga", BoDbTypes.Alpha, Size = 254)]
        public string LocalPathFile { get; set; }

        [FormColumn(4), FieldNoRelated(@"U_EXK_DATE", @"Fecha de carga", BoDbTypes.Date)]
        public DateTime DeliveryDate { get; set; }

        [FormColumn(5), FieldNoRelated(@"U_EXK_FUPL", @"Archivo Anexo", BoDbTypes.Alpha, Size = 254)]
        public string ServerPathFile { get; set; }

        [FormColumn(6), FieldNoRelated(@"U_EXK_BPCD", @"Código Cliente", BoDbTypes.Alpha, Size = 150)]
        public string BusinessPartnerCode { get; set; }

        [FormColumn(7), FieldNoRelated(@"U_EXK_STDS", @"Estado", BoDbTypes.Alpha, Size = 2)]
        [Val(@"SP", @"Procesado"), Val(@"PP", @"Procesado Parcial")]
        public string Status { get; set; }

        [ChildProperty(FUP1.ID)] public List<FUP1> ColumnDetail { get; set; }

        public static class Statuses
        {
            public const string SUCCESS_CODE = "SP";
            public const string PARTIAL_CODE = "PP";
        }
    }
}