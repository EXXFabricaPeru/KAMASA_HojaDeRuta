using System.Collections.Generic;
using GeolocationAPI.Models;

namespace GeolocationAPI.Request
{
    public class AssignRequest
    {
        public List<Route> Routes { get; set; }
        public string Key { get; set; }
    }
}