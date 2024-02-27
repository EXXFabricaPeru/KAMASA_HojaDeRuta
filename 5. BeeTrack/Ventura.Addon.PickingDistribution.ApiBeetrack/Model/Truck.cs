using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Ventura.Addon.ComisionTarjetas.ApiBeetrack.Model
{
    public class Truck
    {
        public string identifier { get; set; }
        public IList<Group> groups { get; set; }
    }
}
