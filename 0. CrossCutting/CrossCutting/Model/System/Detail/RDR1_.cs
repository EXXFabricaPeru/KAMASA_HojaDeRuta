using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail
{
    [SystemTable("CRD1", FormType = "dummy")]
    public class RDR1_
    {
        public string ItemCode { get; set; } = string.Empty;
        public string U_EXK_CFIL { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double PriceAfVat { get; set; }
        public string TaxCode { get; set; }
        public string OcrCode { get; set; }
        public string OcrCode2 { get; set; }
        public string OcrCode3 { get; set; }
        public Dictionary<string, string> UserFields { get; set; } = new Dictionary<string, string>();
    }
}
