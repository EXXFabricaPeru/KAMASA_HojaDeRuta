using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseCounterRepository : Code.Repository
    {
        protected BaseCounterRepository(Company company) : base(company)
        {
        }

        public abstract ResponseDocumentTransaction Register(ODCR entity);

        public abstract Tuple<bool, string> Update(ODCR entity, params Expression<Func<ODCR, object>>[] properties);

        public abstract ResponseTransaction AppendChild(int entry, DCR1 child);

        public abstract Tuple<bool, string> UpdateChild(int entry, DCR1 child, params Expression<Func<DCR1, object>>[] properties);

        public abstract IEnumerable<ODCR> Retrieve(Expression<Func<ODCR, bool>> expression = null);
    }
}