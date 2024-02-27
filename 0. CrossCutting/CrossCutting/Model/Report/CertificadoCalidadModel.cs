using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Report
{
    public class CertificadoCalidadModel
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string CodTransportista { get; set; }
        public string RucTransportista { get; set; }
        public string RutaReferencia { get; set; }
        public string FechaDespacho { get; set; }
        public string NumPedido { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string NombreArticulo { get; set; }
        public string UnidadMedida { get; set; }
        public string CantPedida { get; set; }
        public string CantDespachada { get; set; }
        public string Lote { get; set; }
        public string FechaFab { get; set; }
        public string FechaExp { get; set; }
    
    }
}
