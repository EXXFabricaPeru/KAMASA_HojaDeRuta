using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseBusinessPartnerRepository : Code.Repository
    {
        protected BaseBusinessPartnerRepository(Company company) : base(company)
        {
        }

        public abstract OCRD FindByCode(string cardCode);
    }
}