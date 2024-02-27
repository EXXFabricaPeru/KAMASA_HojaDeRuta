using System;
using System.Collections.Generic;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IDirectionsDomain
    {
        Tuple<decimal, decimal, string> RetrieveBusinessPartnerGeoLocations(string cardCode, string addressCode);

        Tuple<decimal, decimal, string> RetrieveWarehouseGeoLocations(string code);

        IEnumerable<Tuple<string, string>> RetrieveBusinessPartnerDirection(string cardCode);
    }
}