using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventura.Addon.ComisionTarjetas.ApiBeetrack.Model;

namespace Ventura.Addon.ComisionTarjetas.ApiBeetrack.Contracts
{
    public class ResponseGetRoute
    {
        public string status { get; set; }
        public RouteContent response { get; set; }
    }

    public class RouteContent
    {
        public Route route { get; set; }
    }
}
