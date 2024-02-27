using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseDriverRepository : Code.Repository
    {
        protected BaseDriverRepository(Company company) : base(company)
        {
        }

        public abstract BPP_CONDUC FindByCode(string code);

        public abstract BPP_CONDUC FindByLicense(string code);
    }
}