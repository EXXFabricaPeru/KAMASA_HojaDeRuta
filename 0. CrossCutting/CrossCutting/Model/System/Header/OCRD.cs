using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

// ReSharper disable InconsistentNaming

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable(@"OCRD", FormType = "dummy")]
    public class OCRD : BaseSAPTable
    {
        [SAPColumn(@"CardCode")] public string CardCode { get; set; }

        [SAPColumn(@"CardName")] public string CardName { get; set; }

        [SAPColumn(@"Balance")] public decimal Balance { get; set; }

        [SAPColumn(@"LicTradNum")]
        public string LicTradNum { get; set; }

        //[SAPColumn(@"U_BPP_BPTP")] 
        public string BPP_BPTP { get; set; }

        //[SAPColumn(@"U_CL_FACTF", false)]
        //[FieldNoRelated("U_CL_FACTF", "Factura_Fisica?", BoDbTypes.Alpha, Size = 1, Default = "Y")]
        //[Val("N", "NO")]
        //[Val("Y", "SI")]
        public string FACTF { get; set; }

        //[SAPColumn(@"U_CL_CERTIF", false)]
        //[FieldNoRelated("U_CL_CERTIF", "Certificado_Calidad?", BoDbTypes.Alpha, Size = 1, Default = "Y")]
        //[Val("N", "NO")]
        //[Val("Y", "SI")]
        public string CertificadoCalidad { get; set; }

        //[SAPColumn(@"U_CL_CECO", false)]
        //[FieldNoRelated("U_CL_CECO", "Centro de Costo", BoDbTypes.Alpha, Size = 30)]
        public string GenericCostCenter { get; set; }

        //[SAPColumn(@"U_CL_CECO2", false)]
        //[FieldNoRelated("U_CL_CECO2", "Sub Centro de Costo", BoDbTypes.Alpha, Size = 30)]
        public string GenericCostCenter2 { get; set; }

        //[SAPColumn(@"U_CL_CECO3", false)]
        //[FieldNoRelated("U_CL_CECO3", "Sub_Sub Centro de Costo", BoDbTypes.Alpha, Size = 30)]
        public string GenericCostCenter3 { get; set; }

        //[SAPColumn(@"U_CL_CANAL", false)]
        //[FieldLinkedUdt("U_CL_CANAL", "Canal", Size: 2, linkedUserTable:"@VS_LP_CANALVENTA")]
        public string SaleChannel { get; set; }


        [SAPColumn("Phone1")]
        public string Phone { get; set; }

        //[SAPColumn(@"U_VS_AFPRCP")]
        public string TipoAgente { get; set; }

        [SAPColumn(@"WTLiable")]
        public string SujetoRetencion { get; set; }

        public IList<CRD1> Addresses { get; set; }

        public IList<ContactPerson> Contact { get; set; }

        public static class PartnerType
        {
            public const string LEGAL = "TPJ";
            public const string NATURAL = "TPN";
        }
    }
}