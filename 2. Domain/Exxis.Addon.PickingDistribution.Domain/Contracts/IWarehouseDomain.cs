using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IWarehouseDomain
    {
        OWHS RetrieveWarehouse(string code);
    }
}