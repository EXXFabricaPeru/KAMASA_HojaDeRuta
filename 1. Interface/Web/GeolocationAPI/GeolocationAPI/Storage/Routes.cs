using System.Collections.Generic;
using GeolocationAPI.Request;
using LiteDB;

namespace GeolocationAPI.Storage
{
    public class Routes
    {
        public ObjectId Id { get; set; }

        public List<RouteRequest> Route { get; set; }
    }
}