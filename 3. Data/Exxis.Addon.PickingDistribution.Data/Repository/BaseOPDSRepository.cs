using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOPDSRepository : Code.Repository<OPDS>
    {
        protected BaseOPDSRepository(Company company) : base(company)
        {
        }

        public abstract OPDS Update(OPDS setting);

        public abstract OPDS Setting(string id);

        public abstract void ValidData();
    }
}