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
    public class OCRD : BaseUDO
    {
        public const string ID = "VS_PD_OCRD";
        public const string DESCRIPTION = "Certificados";

        [FormColumn(0, Description = "Identificador"), ColumnProperty("Code")]
        public string Code { get; set; }

        [FormColumn(1, Visible = false), ColumnProperty("Name")]
        public string Name { get; set; }

        [FormColumn(2), FieldNoRelated("U_EXK_CDCT", "Código", BoDbTypes.Alpha, Size = 50)]
        public string CertificateId { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_TPCT", "Tipo", BoDbTypes.Alpha, Size = 2)]
        [Val(CertificateTypes.CARNET, "Carnet"), Val(CertificateTypes.CAPACITACION, "Capacitación"), Val(CertificateTypes.CERTIFICADO, "Certificado")]
        public string CertificateType { get; set; }

        [FormColumn(4), FieldNoRelated("U_EXK_DSCT", "Descripción", BoDbTypes.Alpha, Size = 254)]
        public string CertificateDescription { get; set; }

        [FormColumn(5), FieldNoRelated("U_EXK_IVCT", "Inicio Vigencia", BoDbTypes.Date)]
        public DateTime CertificateValidityStart { get; set; }

        [FormColumn(6), FieldNoRelated("U_EXK_FVCT", "Fin Vigencia", BoDbTypes.Date)]
        public DateTime CertificateValidityEnd { get; set; }

        public class CertificateTypes
        {
            //public const string DELIVERY = "DC"; 
            //public const string TRAINING = "TC";
            //public const string PERMIT = "PR";
            public const string CARNET = "CR";
            public const string CERTIFICADO = "CT";
            public const string CAPACITACION = "CP";
        }
    }
}