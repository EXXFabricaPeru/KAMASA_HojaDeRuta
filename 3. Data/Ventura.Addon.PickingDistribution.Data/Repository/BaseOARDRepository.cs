using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOARDRepository : Repository<OARD>
    {
        protected BaseOARDRepository(Company company) : base(company)
        {
        }

        public abstract IEnumerable<OARD> Retrieve(Expression<Func<OARD, bool>> expression = null);

        public abstract IEnumerable<OARD> RetrieveDetail(Expression<Func<ARD1, bool>> expression);

        public abstract IEnumerable<OARD> RetrieveByDate(DateTime date);

        public abstract IEnumerable<OARD> RetrieveBetweenDate(DateTime date,DateTime date2);
        

        public abstract Tuple<bool, string, string> Register(OARD entity);

        public abstract Tuple<bool, string, string> RegisterPrueba(OARD entity,ref List<string>msj);
        

        public abstract Tuple<bool, string> Update(OARD document);

        public abstract Tuple<bool, string> Remove(int documentEntry);

        public abstract Tuple<bool, string> RemoveDetail<T>(int routeEntry) where T : BaseUDO;

        public abstract List<ARD3> RetrieveRelatedDeliveries(string routeEntry, int documentEntry);

        public abstract Tuple<bool, string> InsertDetail<T>(int routeEntry, IEnumerable<T> details) where T : BaseUDO;

        public abstract Tuple<bool, string> UpdateDeliveryStatusNOriginTranshipingRelatedTransferOrder(int entry, int transferOrderIndex, string deliveryStatusCode, int transshippingEntry);
    }
}