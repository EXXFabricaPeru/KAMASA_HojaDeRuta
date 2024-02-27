using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    
    public abstract class BaseOTRDRepository : Code.Repository<OTRD>
    {
        protected BaseOTRDRepository(Company company) : base(company)
        {

        }

        public abstract IEnumerable<OTRD> RetrieveTransporters(Expression<Func<OTRD, bool>> expression = null);
    }
}
