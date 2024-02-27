using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IDriverDomain
    {
        BPP_CONDUC Retrieve(string code);
    }
}