using System;
using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using System.Linq;



namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class LiquidationTransporterDomain 
        : BaseDomain, ILiquidationTransporterDomain
    {
        public LiquidationTransporterDomain(Company company)
            : base(company)
        {
        }

        #region Implementation of ILiquidationTransporterDomain

        public Tuple<bool, string> ReplaceReferenceRoutes(int entry, IEnumerable<LDP1> routes)
        {
            
            
            Tuple<bool, string> response = UnitOfWork.TransporterLiquidateRepository.RemoveChild<LDP1>(entry);
            if (!response.Item1)
                throw new Exception(response.Item2);
            List<LDP1> _routes = new List<LDP1>();
            _routes.AddRange(routes);

            var x = _routes.GroupBy(t => t.RouteEntry).Select(group => group.First()).ToList();

           
            return UnitOfWork.TransporterLiquidateRepository.RegisterChild(entry, x);
        }

        public Tuple<bool, string> ReplaceReferenceMotives(int entry, IEnumerable<LDP2> motives)
        {
            Tuple<bool, string> response = UnitOfWork.TransporterLiquidateRepository.RemoveChild<LDP2>(entry);
            if (!response.Item1)
                throw new Exception(response.Item2);

            return UnitOfWork.TransporterLiquidateRepository.RegisterChild(entry, motives);
        }
        

        public Tuple<bool, string> ClosePayment(int entry, DateTime closeDate)
        {
            return UnitOfWork.TransporterLiquidateRepository.ClosePayment(entry, closeDate);
        }

        public Tuple<bool, string> UpdateHeaderFields(OLDP header)
        {
            return UnitOfWork.TransporterLiquidateRepository.UpdateHeaderFields(header);
        }

        #endregion
    }
}