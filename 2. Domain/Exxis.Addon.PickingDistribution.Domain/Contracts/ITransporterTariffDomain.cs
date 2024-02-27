using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface ITransporterTariffDomain : IBaseDomain
    {
        OATP RetrieveByTransporterCode(string code);

        bool ExistTransporter(string code);
    }
}