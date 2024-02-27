using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header
{
    public class LiquidationSL
    {

        [JsonProperty("Remark")]
        public object Remark { get; set; }

        [JsonProperty("U_VS_LT_CDTD")]
        public string CodigoTienda { get; set; }

        [JsonProperty("U_VS_LT_DSTD")]
        public string NombreTienda { get; set; }

        [JsonProperty("U_VS_LT_PPAY")]
        public string Pasarela { get; set; }

        [JsonProperty("U_VS_LT_CCTR")]
        public string CuentaTransitoria { get; set; }

        [JsonProperty("U_VS_LT_FEIV")]
        public DateTime FechaInicio { get; set; }

        [JsonProperty("U_VS_LT_FEFV")]
        public DateTime FechaFin { get; set; }

        [JsonProperty("U_VS_LT_MONE")]
        public string Moneda { get; set; }

        [JsonProperty("U_VS_LT_ARCH")]
        public string Archivo { get; set; }

        [JsonProperty("U_VS_LT_FEAB")]
        public DateTime? FechaAbono { get; set; }

        [JsonProperty("U_VS_LT_BANK")]
        public string Banco { get; set; }

        [JsonProperty("U_VS_LT_FLCJ")]
        public string FlujoCaja { get; set; }

        [JsonProperty("U_VS_LT_NROP")]
        public string NroOperacion { get; set; }

        [JsonProperty("U_VS_LT_TCAM")]
        public double TipoCambio { get; set; }

        [JsonProperty("U_VS_LT_CCCO")]
        public string CuentaContable { get; set; }

        [JsonProperty("U_VS_LT_IMCO")]
        public double ImporteCobrado { get; set; }

        [JsonProperty("U_VS_LT_OMPA")]
        public double OtroMedioPago { get; set; }

        [JsonProperty("U_VS_LT_COMI")]
        public double Comision { get; set; }

        [JsonProperty("U_VS_LT_COEM")]
        public double ComisionEmisor { get; set; }

        [JsonProperty("U_VS_LT_COMP")]
        public double ComisionMCPeru { get; set; }

        [JsonProperty("U_VS_LT_IGVT")]
        public double IGV { get; set; }

        [JsonProperty("U_VS_LT_NTPA")]
        public double NetoParcial { get; set; }

        [JsonProperty("U_VS_LT_SMCO")]
        public double SumaComisiones { get; set; }

        [JsonProperty("U_VS_LT_IMDE")]
        public double ImporteDepositado { get; set; }

        [JsonProperty("U_VS_LT_IMAJ")]
        public double ImporteAjuste { get; set; }

        [JsonProperty("U_VS_LT_TOGE")]
        public double TotalGeneral { get; set; }

        [JsonProperty("VS_LT_LTR1Collection")]
        public List<LiquidationLineSL> VsLtLtr1Collection { get; set; }
    }
}
