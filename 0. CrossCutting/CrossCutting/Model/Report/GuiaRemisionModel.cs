using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Report
{
    public class GuiaRemisionModel
    {
        public string NumGuia { get; set; }
        public string NomCliente { get; set; }
        public string DirCliente { get; set; }
        public string RucCliente { get; set; }
        public string FechaEmision { get; set; }
        public string FechaDespacho { get; set; }
        public string NumPedido { get; set; }
        public string Vendedor { get; set; }
        public string TipoNumDoc { get; set; }
        public string PtPartida { get; set; }
        public string PtLlegada { get; set; }
        public string NumItem { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public string Lote { get; set; }
        public string Peso { get; set; }
    }
}
