using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Report
{
    public class LiquidationRouteModel
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string Auxiliar { get; set; }
        public string Fecha { get; set; }
        public string Placa { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFinal { get; set; }
        public string CantidadGuia { get; set; }
        public string Contador { get; set; }
        public string NumGuia { get; set; }
        public string NumFactura { get; set; }
        public string RazonSocial { get; set; }
        public string DirEntrega { get; set; }
        public string VentanaMin { get; set; }
        public string VentanaMax { get; set; }

    }
}
