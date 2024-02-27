using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventura.Addon.ComisionTarjetas.Batch.Entity
{
    public class Licencia
    {
        public string CodProducto { get; set; }
        public string InstllNumber { get; set; }
        public string Fechavigencia { get; set; }
        public string Raw { get; set; }

        public Licencia(string value)
        {
            string[] values = value.Split('|');

            CodProducto = values[0];
            InstllNumber = values[1];
            Fechavigencia = values[2];
        }
    }
}
