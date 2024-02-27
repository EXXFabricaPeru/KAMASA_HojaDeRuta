// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseOWHSRepository : Code.Repository<OWHS>
    {
        protected BaseOWHSRepository(Company company) : base(company)
        {
        }

        public abstract OWHS RetrieveWarehouse(string code);

        public abstract IEnumerable<OWHS> Retrieve(Expression<Func<OWHS, bool>> expression);
    }
}