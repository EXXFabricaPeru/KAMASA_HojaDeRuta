using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface ITransporterDomain : IBaseDomain
    {
        IEnumerable<OTRD> RetrieveAll();
        OTRD Retrieve(string code);
        Tuple<bool, string> Liquidate(OARD route);
        OLDP RetrieveLiquidation(int entry);
        Tuple<bool, string> UpdateLiquidateTransporter(OLDP obj);
        IEnumerable<OLDP> RetrieveLiquidationALLByDate_Transporter(DateTime date, string transporter);
    }
}
