using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OACD.ID, 1)]
    public class ACD1
    {
        public const string ID = "VS_PD_ACD1";
        public const string DESCRIPTION = "Certificado Asignado";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_CDCT", "Código", BoDbTypes.Alpha, Size = 50)]
        public string CertificateId { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_TPCT", "Tipo", BoDbTypes.Alpha, Size = 2)]
        [Val(CertificateTypes.CARNET, "Carnet"), Val(CertificateTypes.CAPACITACION, "Capacitación"), Val(CertificateTypes.CERTIFICADO, "Certificado")]
        public string CertificateType { get; set; }

       

        [EnhancedColumn(3), FieldNoRelated("U_EXK_DSCT", "Descripción", BoDbTypes.Alpha, Size = 254)]
        public string CertificateDescription { get; set; }


        public class CertificateTypes
        {
            public const string CARNET = "CR";
            public const string CERTIFICADO = "CT";
            public const string CAPACITACION = "CP";
        }
    }
}