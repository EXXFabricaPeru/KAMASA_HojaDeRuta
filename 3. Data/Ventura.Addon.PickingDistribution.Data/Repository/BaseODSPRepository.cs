using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseODSPRepository : Code.Repository<ODSP>
    {
        protected BaseODSPRepository(Company company) : base(company)
        {
        }

        public abstract IEnumerable<ODSP> RetrieveAll();
    }
}