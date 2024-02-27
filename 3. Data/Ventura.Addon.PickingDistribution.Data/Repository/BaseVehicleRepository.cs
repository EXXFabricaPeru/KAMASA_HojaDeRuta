using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseVehicleRepository : Code.Repository
    {
        protected BaseVehicleRepository(Company company) : base(company)
        {
        }

        public abstract BPP_VEHICU FindByCode(string code);
    }
}