using System;
using System.Collections.Generic;
using GeolocationAPI.Models;

namespace GeolocationAPI.Utilities
{
    public class VehicleComparer : IEqualityComparer<Vehicle>
    {
        public bool Equals(Vehicle x, Vehicle y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Code == y.Code;
        }

        public int GetHashCode(Vehicle obj)
        {
            return HashCode.Combine(obj.Code, obj.Name, obj.IsAvailable, obj.WeightCapacity, obj.VolumeCapacity, obj.DistributionType, obj.HasRefrigeration);
        }
    }
}