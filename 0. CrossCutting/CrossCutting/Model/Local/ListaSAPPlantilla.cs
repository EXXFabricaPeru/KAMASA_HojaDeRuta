using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Local
{
    public  class ListaSAPPlantilla
    {
        public static List<(string, string)> staticTupleList = new List<(string, string)>
    {
        ("U_VS_LT_NREF", "Nro. Referencia de Cobro"),
        ("U_VS_LT_FECO", "Fecha Cobro"),
        ("U_VS_LT_NTAR", "Nro.  de Tarjeta"),
        ("U_VS_LT_IMTA", "Importe Cobrado Tarjeta"),
        ("U_VS_LT_COMI", "Comisión"),
        ("U_VS_LT_COEM", "Comisión Emisor"),
        ("U_VS_LT_COMP", "Comisión MCPeru"),
        ("U_VS_LT_IGVT", "IGV"),
        ("U_VS_LT_NTPA", "Neto Parcial")
        // Puedes agregar más elementos según sea necesario
    };
    }
}
