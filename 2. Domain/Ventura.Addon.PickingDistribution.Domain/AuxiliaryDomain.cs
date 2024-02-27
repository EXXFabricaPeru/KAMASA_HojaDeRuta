using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class AuxiliaryDomain : BaseDomain, IAuxiliaryDomain
    {
        public AuxiliaryDomain(Company company)
            : base(company)
        {
        }

        public OHEM FindByCode(string cardCode)
        {
            return UnitOfWork.EmployeeRepository.FindByCode(cardCode);
        }
    }
}