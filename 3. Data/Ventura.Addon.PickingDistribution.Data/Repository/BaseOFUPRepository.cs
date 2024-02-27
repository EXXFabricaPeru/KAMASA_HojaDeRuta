using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseOFUPRepository : Code.Repository<OFUP>
    {
        protected BaseOFUPRepository(Company company) : base(company)
        {

        }

        public abstract IEnumerable<OFUP> Retrieve(Expression<Func<OFUP, bool>> expression = null);
        public abstract Tuple<bool, string, int, int> Register(OFUP entity);
        public abstract Tuple<bool, string> InsertLineEDIFile(int entry, IEnumerable<FUP1> lines);
        public abstract Tuple<bool, string> UpdateStatusEDIFile(int entry, string status);

        public abstract void CreaterOrder(ref ORDR_ OrdenVenta, out int DocEntry, out string mensaje);
        public abstract void LoadFile(int docentry, string archivo);
    }
}