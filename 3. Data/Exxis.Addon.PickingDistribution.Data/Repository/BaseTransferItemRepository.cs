using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseTransferItemRepository : Code.Repository<OWTR>
    {
        protected BaseTransferItemRepository(Company company) 
            : base(company)
        {
        }

        public abstract Tuple<bool, string> Register(OWTR entity);

        public abstract IEnumerable<OWTR> Retrieve(Expression<Func<OWTR, bool>> expression = null);

        public abstract Tuple<bool, string> UpdateBaseEntity(OWTR document);
        public abstract Tuple<bool, string> UpdateBaseEntityRequest(OWTR document);

        public abstract Tuple<bool, string> UpdateFolio(OWTR document);
        public abstract Tuple<bool, string> UpdateFolioRequest(OWTR document);

        public abstract OWTR RetrieveTransfeStockByOT(Expression<Func<OWTR, bool>> expression);

        public abstract OWTR RetrieveRequestTransfeStockByOT(Expression<Func<OWTR, bool>> expression);
    }
}