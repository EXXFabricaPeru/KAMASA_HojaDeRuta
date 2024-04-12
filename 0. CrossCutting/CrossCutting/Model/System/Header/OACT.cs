using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VersionDLL.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable(@"OACT", FormType = "dummy")]
    public class OACT : BaseSAPTable
    {
        [SAPColumn(@"AcctCode")] public string Cuenta { get; set; }

        [SAPColumn(@"AcctName")] public string NombreCuenta { get; set; }

        [SAPColumn(@"AcctCurr")] public string Moneda { get; set; }

        [SAPColumn(@"U_VS_LT_PASP", false)]
        [FieldLinkedUdt("U_VS_LT_PASP", "Pasarela de Pago", Size: 15, linkedUserTable: "@VS_LT_OPAP")]
        public string PasarelaPago { get; set; }

        [SAPColumn(@"U_VS_LT_COTI", false)]
        [FieldLinkedUdt("U_VS_LT_COTI", "Tienda", Size: 10, linkedUserTable: "@CL_CODEMP")]
        public string Tienda { get; set; }
    }
}
