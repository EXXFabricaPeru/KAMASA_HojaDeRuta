using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class DirectionsDomain : BaseDomain, IDirectionsDomain
    {
        public DirectionsDomain(Company company) : base(company)
        {
        }



        public Tuple<decimal, decimal, string> RetrieveBusinessPartnerGeoLocations(string cardCode, string addressCode)
        {
            UnitOfWork.BusinessPartnerRepository.SetRetrieveFormat(RetrieveFormat.Complete);
            var businessPartner = UnitOfWork.BusinessPartnerRepository.FindByCode(cardCode);
            var address = businessPartner.Addresses.Single(t => t.Code == addressCode && t.Type == CRD1.AddressType.SHIPPING);
            return Tuple.Create(address.Latitude, address.Longitude, address.Street);
        }

        public Tuple<decimal, decimal, string> RetrieveWarehouseGeoLocations(string code)
        {
            UnitOfWork.BusinessPartnerRepository.SetRetrieveFormat(RetrieveFormat.Complete);
            var warehouse = UnitOfWork.WarehouseRepository.RetrieveWarehouse(code);
            return Tuple.Create(warehouse.Latitude, warehouse.Longitude, warehouse.Street);
        }

        public IEnumerable<Tuple<string, string>> RetrieveBusinessPartnerDirection(string cardCode)
        {
            UnitOfWork.BusinessPartnerRepository.SetRetrieveFormat(RetrieveFormat.Complete);
            var businessPartner = UnitOfWork.BusinessPartnerRepository.FindByCode(cardCode);
            var address = businessPartner.Addresses.Where(t => t.Type == CRD1.AddressType.SHIPPING);
            IList<Tuple<string, string>> result = new List<Tuple<string, string>>();

            foreach (var item in address)
            {
                result.Add(Tuple.Create(item.Code, item.Code));
            }
            return result;
        }
    }
}