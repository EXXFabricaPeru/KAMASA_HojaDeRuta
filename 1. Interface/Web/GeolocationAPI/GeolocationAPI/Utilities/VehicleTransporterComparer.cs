using System;
using System.Collections.Generic;
using GeolocationAPI.Models;

namespace GeolocationAPI.Utilities
{
    public class VehicleTransporterComparer : IEqualityComparer<Transporter_Vehicle>
    {
        public bool Equals(Transporter_Vehicle x, Transporter_Vehicle y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Value[0].VSPDOTRDVSPDTRD2Collection.Code == y.Value[0].VSPDOTRDVSPDTRD2Collection.Code;
        }

        public int GetHashCode(Transporter_Vehicle obj)
        {
            return HashCode.Combine(obj.Value[0].VSPDOTRDVSPDTRD2Collection.Code, obj.Value[0].VSPDOTRDVSPDTRD2Collection.Name,
                obj.Value[0].VSPDOTRDVSPDTRD2Collection.IsAvailable, 
                obj.Value[0].VSPDOTRDVSPDTRD2Collection.WeightCapacity, 
                obj.Value[0].VSPDOTRDVSPDTRD2Collection.VolumeCapacity, 
                //obj.Value[0].VSPDOTRDVSPDTRD2Collection.DistributionType, 
                obj.Value[0].VSPDOTRDVSPDTRD2Collection.HasRefrigeration);
        }
    }
}