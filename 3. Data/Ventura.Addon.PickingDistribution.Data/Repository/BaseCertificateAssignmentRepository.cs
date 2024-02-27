using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseCertificateAssignmentRepository : Code.Repository<OACD>
    {
        protected BaseCertificateAssignmentRepository(Company company)
            : base(company)
        {
        }

        public abstract IEnumerable<OACD> Retrieve(Expression<Func<OACD, bool>> expression);
    }
}