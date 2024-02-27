using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseTariffMotiveRepository : Code.Repository<OTMT>
    {
        protected BaseTariffMotiveRepository(SAPbobsCOM.Company company) 
            : base(company)
        {
        }

        public abstract IEnumerable<OTMT> Retrieve(Expression<Func<OTMT, bool>> expression);
    }
}