using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;


namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IBeetrackDomain : IBaseDomain
    {
        Tuple<string,string> GetCredentials();

        bool IsEnable();
    }
}
