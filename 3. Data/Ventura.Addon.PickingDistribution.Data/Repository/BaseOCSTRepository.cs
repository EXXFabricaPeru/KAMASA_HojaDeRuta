using SAPbobsCOM;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOCSTRepository : Repository<OCST>
    {
        protected BaseOCSTRepository(Company company) : base(company)
        {
        }

        public abstract List<OCST> ListDepartments { get; }
    }
}