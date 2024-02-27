using LiteDB;

namespace GeolocationAPI.Storage
{
    public class AvailableVehicles
    {
        public ObjectId Id { get; set; }
        public string[] VehiclesIds { get; set; }
        public bool IsActive { get; set; }
    }
}