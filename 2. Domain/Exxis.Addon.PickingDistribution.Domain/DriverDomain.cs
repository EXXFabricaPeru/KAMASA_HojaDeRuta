using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class DriverDomain : BaseDomain, IDriverDomain
    {
        public DriverDomain(Company company) : base(company)
        {
        }

        public BPP_CONDUC Retrieve(string code) 
            => UnitOfWork.DriverRepository.FindByCode(code);
    }
}