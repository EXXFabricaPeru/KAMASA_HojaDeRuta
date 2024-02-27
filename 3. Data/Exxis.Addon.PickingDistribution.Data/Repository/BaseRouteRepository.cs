using System;
using SAPbobsCOM;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseRouteRepository : Code.Repository<OARD>
    {
        protected BaseRouteRepository(Company company) : base(company)
        {
        }

        public abstract Tuple<bool, string> Register(OARD entity);

        public abstract IEnumerable<OARD> RetrieveDisapprovalReasons();

        //public abstract IEnumerable<OARD> RetrieveReasons();

        public abstract OARD RetrieveDisapprovalReasonByCode(string code);

        public abstract string RetrieveDescriptionOfValidValueCode(string field, string code);

        public abstract IEnumerable<Tuple<string, string>> RetrieveSaleChannels();
    }
}