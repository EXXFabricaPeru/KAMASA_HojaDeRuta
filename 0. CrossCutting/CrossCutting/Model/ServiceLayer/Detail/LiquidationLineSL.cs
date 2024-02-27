using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail
{
    public class LiquidationLineSL
    {

        [JsonProperty("LineId")]
        public int LineId { get; set; }

        [JsonProperty("U_VS_LT_CDTD")]
        public string CodigoTienda { get; set; }

        [JsonProperty("U_VS_LT_PPAY")]
        public string Pasarela { get; set; }

        [JsonProperty("U_VS_LT_CCTR")]
        public string CuentaTransitoria { get; set; }

        [JsonProperty("U_VS_LT_TDOC")]
        public string TipoDocumento { get; set; }

        [JsonProperty("U_VS_LT_NTKT")]
        public string NroTicket { get; set; }

        [JsonProperty("U_VS_LT_COCL")]
        public string CodigoCliente { get; set; }

        [JsonProperty("U_VS_LT_NOCL")]
        public string NombreCliente { get; set; }

        [JsonProperty("U_VS_LT_FEDO")]
        public DateTime? FechaDocumento { get; set; }

        [JsonProperty("U_VS_LT_IMDO")]
        public double? ImporteDocumento { get; set; }

        [JsonProperty("U_VS_LT_MONE")]
        public string Moneda { get; set; }

        [JsonProperty("U_VS_LT_NSAP")]
        public int? NroSAPCobro { get; set; }

        [JsonProperty("U_VS_LT_NREF")]
        public string NroReferencia { get; set; }

        [JsonProperty("U_VS_LT_FECO")]
        public DateTime? FechaCobrado { get; set; }

        [JsonProperty("U_VS_LT_NTAR")]
        public string NroTarjeta { get; set; }

        [JsonProperty("U_VS_LT_IMTA")]
        public double? ImporteTarjeta { get; set; }

        [JsonProperty("U_VS_LT_CONC")]
        public int? CodigoDocumento { get; set; }

        [JsonProperty("U_VS_LT_OMPA")]
        public double? OtroMedioPago { get; set; }

        [JsonProperty("U_VS_LT_COMI")]
        public double? Comision { get; set; }

        [JsonProperty("U_VS_LT_COEM")]
        public double? ComisionEmisor { get; set; }

        [JsonProperty("U_VS_LT_COMP")]
        public double? ComisionMCPeru { get; set; }

        [JsonProperty("U_VS_LT_IGVT")]
        public double? IGV { get; set; }

        [JsonProperty("U_VS_LT_NTPA")]
        public double? NetoParcial { get; set; }

        [JsonProperty("U_VS_LT_MOAJ")]
        public string MotivoAjuste { get; set; }

        [JsonProperty("U_VS_LT_CCAS")]
        public string CuentaAsignada { get; set; }

        [JsonProperty("U_VS_LT_SNAS")]
        public string SocioNegocioAsociado { get; set; }

        [JsonProperty("U_VS_LT_IMAJ")]
        public double? ImporteAjuste { get; set; }

        [JsonProperty("U_VS_LT_NROP")]
        public string NroOperacion { get; set; }

        [JsonProperty("U_VS_LT_STAD")]
        public string Estado { get; set; }

        [JsonProperty("U_VS_LT_TCAM")]
        public double? TipoCambio { get; set; }


    }
}
