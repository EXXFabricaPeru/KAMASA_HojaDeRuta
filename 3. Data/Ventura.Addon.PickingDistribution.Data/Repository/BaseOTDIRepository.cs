using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOTDIRepository : Code.Repository<OTMI>
    {
        protected BaseOTDIRepository(Company company) : base(company)
        {
        }

        public abstract IEnumerable<OTMI> MappingValue(string originName);
    }
}
