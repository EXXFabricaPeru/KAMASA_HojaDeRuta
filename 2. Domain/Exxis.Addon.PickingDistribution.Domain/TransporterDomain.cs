using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Z.Core.Extensions;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class TransporterDomain : BaseDomain, ITransporterDomain
    {
        public TransporterDomain(Company company) : base(company)
        {
        }

        public IEnumerable<OTRD> RetrieveAll()
        {
            return UnitOfWork.TransporterRepository.RetrieveTransporters();
        }

        public OTRD Retrieve(string code)
        {
            try
            {
                return UnitOfWork.TransporterRepository
                    .RetrieveTransporters(t => t.SupplierId == code)
                    .TryFirst(t => true, exception => { throw new Exception($"No existe el transportista con código '{code}'."); });
            }
            catch (Exception)
            {

                return null;
            }

        }

        public Tuple<bool, string> Liquidate(OARD route)
        {
            try
            {
                string supplierId = route.SupplierId;
                OLDP liquidation = UnitOfWork.TransporterLiquidateRepository
                    .Retrieve(t => t.SupplierId == supplierId && t.PaymentStatus == OLDP.Status.PENDING)
                    .SingleOrDefault();

                var isNew = false;
                if (liquidation == null)
                {
                    liquidation = make_default_liquidation(supplierId);
                    isNew = true;
                }

                liquidation.RelatedRoutes.Add(append_liquidation_detail(supplierId, route));
                liquidation.TotalAmount = liquidation.RelatedRoutes.Sum(t => t.TotalAmount);
                return isNew
                    ? UnitOfWork.TransporterLiquidateRepository.Register(liquidation)
                    : UnitOfWork.TransporterLiquidateRepository.Update(liquidation);
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, exception.Message);
            }
        }

        public OLDP RetrieveLiquidation(int entry)
        {
            return UnitOfWork.TransporterLiquidateRepository
                .Retrieve(t => t.DocumentEntry == entry)
                .Single();
        }

        private OLDP make_default_liquidation(string supplierId)
        {
            OTRD transporter = UnitOfWork.TransporterRepository.RetrieveTransporters(t => t.SupplierId == supplierId)
                .Single();
            return new OLDP
            {
                SupplierId = transporter.SupplierId,
                SupplierName = transporter.SupplierName,
                RelatedRoutes = new List<LDP1>()
            };
        }

        private LDP1 append_liquidation_detail(string supplierId, OARD route)
        {
            OATP tariff = UnitOfWork.TariffRepository.Retrieve(t => t.SupplierId == supplierId)
                .TrySingle(t => true,
                    exception => { throw new Exception($"El transportista '{supplierId}' no tiene tarifario configurado."); });

            var liquidationDetail = new LDP1
            {
                RouteEntry = route.DocumentEntry.ToString(),
                RouteNumber = route.DocumentNumber,
                TotalPoints = route.RelatedTransferOrders.Count,
                NotDeliveredPoints = route.RelatedTransferOrders.Count(t =>
                    t.DeliveryStatus == ARD1.Status.NOT_DELIVERED || t.DeliveryStatus == DistributionDeliveryStatus.RESCHEDULED_TRANS ||
                    t.DeliveryStatus == DistributionDeliveryStatus.TRANSSHIPPING)
            };

            Func<ATP1, bool> secondFilter = route.RateType == TransporterTariffRateType.FLATT
                ? (Func<ATP1, bool>) (tariffLine => tariffLine.Code == route.CodeType)
                : (Func<ATP1, bool>) (tariffLine => tariffLine.DeliveredPoints == route.CodeType.ToInt32());

            ATP1 relatedTariff = tariff.RelatedTariffs
                .Where(tariffLine => tariffLine.RateType == route.RateType)
                .TrySingle(secondFilter,
                    exception =>
                    {
                        throw new Exception($"El transportista '{supplierId}' no tiene tarifario para el tipo de ruta '{route.RateType}'.");
                    });

            liquidationDetail.DeliveredPoints = liquidationDetail.TotalPoints - liquidationDetail.NotDeliveredPoints;
            liquidationDetail.TotalValue = relatedTariff.TotalValue;
            liquidationDetail.TotalAmount = liquidationDetail.TotalValue - liquidationDetail.TotalDiscounts;
            return liquidationDetail;
        }

        public Tuple<bool, string> UpdateLiquidateTransporter(OLDP obj)
        {
            return UnitOfWork.TransporterLiquidateRepository.Update(obj);
        }

        public IEnumerable<OLDP> RetrieveLiquidationALLByDate_Transporter(DateTime date, string transporter)
        {
            return UnitOfWork.TransporterLiquidateRepository
                .RetrieveByDate(date,transporter);
        }
    }
}