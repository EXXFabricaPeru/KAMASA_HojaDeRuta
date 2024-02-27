using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOADMRepository : Repository<OADM>
    {
        protected BaseOADMRepository(Company company) : base(company)
        {
        }

        public abstract OADM CurrentCompany { get; }
    }
}