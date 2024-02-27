using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseORTRRepository : Repository<ORTR>
    {
        protected BaseORTRRepository(Company company) : base(company)
        {
        }

        public abstract Tuple<bool, string, string> Register(ORTR entity);

        public abstract IEnumerable<ORTR> Retrieve(Expression<Func<ORTR, bool>> expression);

        public abstract ORTR RetrieveDocEntryRelated(int documentEntry);

        public abstract Tuple<bool, string> UpdateBaseEntity(ORTR document);

        public abstract Tuple<bool, string> Close(int documentEntry);

        public abstract Tuple<bool, string> Postponed(int documentEntry);

        public abstract Tuple<bool, string> Remove(int documentEntry);

        public abstract IEnumerable<ORTR> RetrieveByRoute(int codRoute);

        public abstract Tuple<bool, string> DeleteBatchRelated(ORTR document);

        public abstract Tuple<bool, string> DeleteBatchRelatedByItem(ORTR document,RTR3 documentLine);
        
        public abstract Tuple<bool, string> AddBatchRelated(ORTR document);
    }
}