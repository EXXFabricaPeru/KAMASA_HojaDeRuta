using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseDistributionHistoryRepository : Code.Repository<OHDM>
    {
        protected BaseDistributionHistoryRepository(Company company) : base(company)
        {
        }

        public abstract Tuple<bool, string> Register(OHDM document);

        public abstract Tuple<bool, string> Update(OHDM document);

        public abstract Tuple<bool, string> Remove(int documentEntry);

        public abstract Tuple<bool, string> RemoveLine(int documentEntry, int lineId);

        public abstract IEnumerable<OHDM> Retrieve(Expression<Func<OHDM, bool>> expression);
    }
}