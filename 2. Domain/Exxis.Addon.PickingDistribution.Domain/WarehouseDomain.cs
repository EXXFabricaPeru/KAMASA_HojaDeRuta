using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class WarehouseDomain : BaseDomain, IWarehouseDomain
    {
        public WarehouseDomain(Company company) : base(company)
        {
        }

        public OWHS RetrieveWarehouse(string code)
        {
            return UnitOfWork.WarehouseRepository.RetrieveWarehouse(code);
        }
    }
}