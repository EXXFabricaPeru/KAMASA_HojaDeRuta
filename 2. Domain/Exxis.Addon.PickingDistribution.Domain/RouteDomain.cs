using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ApiResponse;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using Z.Core.Extensions;
using Newtonsoft.Json;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class RouteDomain : BaseDomain, IRouteDomain
    {
        public RouteDomain(Company company)
            : base(company)
        {
        }

        public Tuple<bool, string> Register(OARD route)
        {
            Tuple<bool, string, string> register = UnitOfWork.RouteRepository.Register(route);
            return Tuple.Create(register.Item1, register.Item2);
        }

        public OARD Retrieve(int documentEntry)
        {
            return UnitOfWork.RouteRepository.Retrieve(t => t.DocumentEntry == documentEntry).Single();
        }

        public OARD RetrievebyOT(int documentEntry)
        {
            return UnitOfWork.RouteRepository.RetrieveDetail(t => t.TransferOrderDocEntry == documentEntry).Single();
        }

        public IEnumerable<OARD> RetrieveInProgressRoutes()
        {
            return UnitOfWork.RouteRepository.Retrieve(t => t.RouteStatus == OARD.Status.IN_PROGRESS);
        }

        public IEnumerable<OARD> RetrieveRoutesbyDate(DateTime _startDate, DateTime _endDate)
        {
            return UnitOfWork.RouteRepository.RetrieveBetweenDate(_startDate, _endDate);
        }

        public IEnumerable<OARD> SyncRoutesWithThirdPartySoftware(IEnumerable<OARD> routes)
        {
            throw new NotImplementedException();
        }



        private void append_marketing_documents(OARD parsedRoute)
        {
            parsedRoute.RelatedMarketingDocuments = new List<ARD2>();
            foreach (var ot in parsedRoute.RelatedTransferOrders)
            {
                var transferOrder = UnitOfWork.TransferOrderRepository.Retrieve(t => t.DocumentEntry == ot.TransferOrderDocEntry).Single();

                foreach (var document in transferOrder.RelatedSAPDocuments)
                {
                    parsedRoute.RelatedMarketingDocuments.Add(new ARD2
                    {
                        TransferOrderDocEntry = transferOrder.DocumentEntry,
                        TransferOrderNumber = transferOrder.DocumentNumber,
                        ReferenceDocumentEntry = document.DCEN,
                        ReferenceDocumentNumber = document.DCNM,
                        ReferenceDocumentSAPType = 22,
                        DeliveryStatus = DistributionDeliveryStatus.IN_PROGRESS
                    });
                }
            }
        }

        public Tuple<bool, string> UpdateRoute(OARD route)
        {
            return UnitOfWork.RouteRepository.Update(route);
        }

        public Tuple<bool, string> RemoveDetail<T>(int routEntry) where T : BaseUDO
        {
            return UnitOfWork.RouteRepository.RemoveDetail<T>(routEntry);
        }

        public Tuple<bool, string> CloseRoute(int entry)
        {
            var route = new OARD
            {
                DocumentEntry = entry,
                RouteStatus = OARD.Status.CLOSED
            };
            return UnitOfWork.RouteRepository.Update(route);
        }

        public Tuple<bool, string> LiquidateRoute(OARD route)
        {
            try
            {
                IEnumerable<ARD1> transferOrdersWithOutTransshipping = route.RelatedTransferOrders
                    .Where(t => t.DeliveryStatus != DistributionDeliveryStatus.TRANSSHIPPING );


                foreach (ARD1 relatedTransferOrder in transferOrdersWithOutTransshipping)
                {
                    IEnumerable<ARD2> relatedDocuments = route.RelatedMarketingDocuments
                        .Where(t => t.TransferOrderDocEntry == relatedTransferOrder.TransferOrderDocEntry && t.GeneracionStatus!=DistributionDeliveryStatus.ANULADO_DESCRIPTION);

                    foreach (ARD2 document in relatedDocuments)
                    {
                        var listRelatedOvs = relatedDocuments.Where(t => t.TransferOrderDocEntry == document.TransferOrderDocEntry &&
                          (t.DeliveryStatus == DistributionDeliveryStatus.RESCHEDULED || t.DeliveryStatus == DistributionDeliveryStatus.RESCHEDULED_TRANS)).Select(t => t.ReferenceDocumentEntry);

                        if (document.ReferenceNumberAtCardDelivery == "09G006-00076533")
                        {
                            var x = 213;
                        }
                        //RouteCloser.RouteCloser.RelatedDelivery relatedDelivery = make_related_delivery(route, relatedTransferOrder, document, listRelatedOvs.ToList());
                        //var routeCloser = RouteCloser.RouteCloser.MakeCloser(relatedDelivery, UnitOfWork);
                        //routeCloser.RegisterHistoryData();
                        //routeCloser.UpdateRootDocument();
                        //routeCloser.CloseDocument(); //TODO: ID 40 Si la liquidacion viene de GR+Fact. No envia el rechazo a "Solicitud de Devolucion"
                        //routeCloser.TransferToObservationWarehouseIfNecessary();
                    }
                    if (route.OriginFlow != DistributionFlow.SERVICE_SALE)
                        relatedTransferOrder.DeliveryStatus = get_transfer_order_status(relatedDocuments);

                    ORTR OT = new ORTR();
                    ORTR transferOrder = UnitOfWork.TransferOrderRepository.Retrieve(t => t.DocumentEntry == relatedTransferOrder.TransferOrderDocEntry).FirstOrDefault();
                    //transferOrder = _transferOrderDomain.RetrieveOrder(relatedTransferOrder.TransferOrderDocEntry);
                    //transferOrder.DocumentEntry = relatedTransferOrder.TransferOrderDocEntry;
                    transferOrder.STAD = "CL";

                    Tuple<bool, string> response = UnitOfWork.TransferOrderRepository.UpdateBaseEntity(transferOrder);
                    if (!response.Item1)
                        throw new Exception(response.Item2);
                }

                UnitOfWork.RouteRepository.Update(route);

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, exception.Message);
            }
        }

        //private RouteCloser.RouteCloser.RelatedDelivery make_related_delivery(OARD route, ARD1 transferOrder, ARD2 document, List<int> listOvs = null)
        //{
        //    var relatedDelivery = new RouteCloser.RouteCloser.RelatedDelivery();
        //    relatedDelivery.TransferOrderShipped = transferOrder;
        //    relatedDelivery.DocumentShipped = document;
        //    relatedDelivery.referenceOT = listOvs;
        //    relatedDelivery.DocumentLinesShipped = route.RelatedDeliveredArticles
        //        .Where(t => t.TransferOrderEntry == document.TransferOrderDocEntry && t.ReferenceDocumentEntry == document.ReferenceDocumentEntry)
        //        .ToList();
        //    return relatedDelivery;
        //}

        private string get_transfer_order_status([NoEnumeration] IEnumerable<ARD2> relatedDocuments)
        {
            if (relatedDocuments.Any(t => t.DeliveryStatus == DistributionDeliveryStatus.NOT_DELIVERED))
                return ARD1.Status.NOT_DELIVERED;

            if (relatedDocuments.All(t => t.DeliveryStatus == DistributionDeliveryStatus.DELIVERED || t.DeliveryStatus == DistributionDeliveryStatus.TRANSFERRED))
                return ARD1.Status.DELIVERED;

            if (relatedDocuments.All(t => t.DeliveryStatus == DistributionDeliveryStatus.REJECTED))
                return ARD1.Status.REJECTED;

            if (relatedDocuments.All(t => t.DeliveryStatus == DistributionDeliveryStatus.ASSIGN))
                return ARD1.Status.DELIVERED;

            if (relatedDocuments.All(t => t.DeliveryStatus == DistributionDeliveryStatus.RESCHEDULED))
                return DistributionDeliveryStatus.RESCHEDULED;

            if (relatedDocuments.All(t => t.DeliveryStatus == DistributionDeliveryStatus.RESCHEDULED_TRANS))
                return DistributionDeliveryStatus.RESCHEDULED_TRANS;

            return ARD1.Status.PARTIAL_REJECTED;
        }

        public Tuple<bool, string> TryRevertClosingRoute(OARD route)
        {
            route.RouteStatus = OARD.Status.IN_PROGRESS;
            UnitOfWork.RouteRepository.Update(route);
            return Tuple.Create(true, string.Empty);
        }

        public int RetrieveLastDailyCounter(DateTime date)
        {
            ODCR dailyCounter = UnitOfWork.CounterRepository.Retrieve(t => t.DayCounter == date)
                .SingleOrDefault();

            if (dailyCounter == null)
                return default(int);

            return dailyCounter.UsedCounters
                .OrderByDescending(t => t.Counter)
                .First()
                .Counter;
        }


        private int retrieve_route_series(string originFlow)
        {
            OPDS seriesSettings = null;
            switch (originFlow)
            {
                case DistributionFlow.ITEM_SALE:
                    seriesSettings = UnitOfWork.SettingsRepository.Setting(OPDS.Codes.SALE_ROUTE_SERIES);
                    break;
                case DistributionFlow.SERVICE_SALE:
                    seriesSettings = UnitOfWork.SettingsRepository.Setting(OPDS.Codes.SERVICE_ROUTE_SERIES);
                    break;
                case DistributionFlow.INVENTORY_TRANSFER:
                    seriesSettings = UnitOfWork.SettingsRepository.Setting(OPDS.Codes.TRANSFER_ROUTE_SERIES);
                    break;
                case DistributionFlow.RETURN:
                    seriesSettings = UnitOfWork.SettingsRepository.Setting(OPDS.Codes.RETURN_ROUTE_SERIES);
                    break;
                case DistributionFlow.PURCHASE:
                    seriesSettings = UnitOfWork.SettingsRepository.Setting(OPDS.Codes.PURCHASE_ROUTE_SERIES);
                    break;
                default:
                    throw new Exception("[Error] Origin Flow not supported");
            }
            return seriesSettings.Value.ToInt32();
        }

        public IEnumerable<OARD> RetrieveOpenedRoutesByDate(DateTime date)
        {
            return UnitOfWork.RouteRepository.RetrieveByDate(date);
        }

        public Tuple<bool, string> AppendDetails(int entry, IEnumerable<ARD1> transferOrdersRelated, IEnumerable<ARD2> documentsRelated)
        {
            BaseOARDRepository repository = UnitOfWork.RouteRepository;
            Tuple<bool, string> response = repository.InsertDetail(entry, transferOrdersRelated);
            return !response.Item1 ? response : repository.InsertDetail(entry, documentsRelated);
        }

        public Tuple<bool, string> ChangeToTransshipping(int entry, int transferOrdersRelatedIndex, int originTransshipping)
        {
            BaseOARDRepository repository = UnitOfWork.RouteRepository;
            return repository.UpdateDeliveryStatusNOriginTranshipingRelatedTransferOrder(entry, transferOrdersRelatedIndex, DistributionDeliveryStatus.TRANSSHIPPING, originTransshipping);
        }

        public Tuple<bool, string> SaveRoute(RouteBuildResponse route, DateTime distribuitionDate, ref List<string> msj)

        {

            OARD parsedRoute = parse_to_sap_object(route, distribuitionDate);
            if (parsedRoute.EndHour.Value.Day > 1)
                parsedRoute.EndHour = parsedRoute.EndHour.Value.AddDays(-1);
            if (parsedRoute.OriginFlow == DistributionFlow.PURCHASE || parsedRoute.OriginFlow == DistributionFlow.INVENTORY_TRANSFER)
                append_marketing_documents(parsedRoute);

            if (parsedRoute.OriginFlow == DistributionFlow.SERVICE_SALE || parsedRoute.OriginFlow == DistributionFlow.PURCHASE || parsedRoute.OriginFlow == DistributionFlow.INVENTORY_TRANSFER)
                parsedRoute.RouteStatus = OARD.Status.IN_PROGRESS;
            if (parsedRoute.OriginFlow == DistributionFlow.RETURN)
                parsedRoute.RouteStatus = OARD.Status.PROGRAMED;
            //JsonSerializerSettings settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //};

            //msj.Add(JsonConvert.SerializeObject(parsedRoute, settings));
            //foreach (var item in parsedRoute.RelatedTransferOrders)
            //{
            //    msj.Add(JsonConvert.SerializeObject(item, settings));
            //}

            //msj = JsonConvert.SerializeObject(parsedRoute);

            //return Tuple.Create(false,"");
            Tuple<bool, string, string> registerResult = UnitOfWork.RouteRepository.Register(parsedRoute);
            //Tuple<bool, string, string> registerResult = UnitOfWork.RouteRepository.RegisterPrueba(parsedRoute);

            if (!registerResult.Item1)
                return Tuple.Create(registerResult.Item1, registerResult.Item2);

            foreach (ARD1 transferOrderRelated in parsedRoute.RelatedTransferOrders)
            {
                ORTR transferOrder = UnitOfWork.TransferOrderRepository
                    .Retrieve(t => t.DocumentEntry == transferOrderRelated.TransferOrderDocEntry)
                    .Single();

                transferOrder.STAD = ORTR.Status.PLANNING;
                var respuesta = UnitOfWork.TransferOrderRepository.UpdateBaseEntity(transferOrder);
                //msj = msj + " - " + "Paso 4 " + respuesta.Item2;
            }

            ODCR counter = UnitOfWork.CounterRepository
                .Retrieve(t => t.DayCounter == distribuitionDate)
                .SingleOrDefault();

            var child = new DCR1
            {
                Counter = route.ReferenceCounter,
                RouteEntry = registerResult.Item2.ToInt32(),
                RouteNumber = registerResult.Item3.ToInt32()
            };



            //if (child.Counter == 8)
            //    return Tuple.Create(false, "error de prueba ");



            if (counter == null)
            {
                counter = new ODCR();
                counter.DayCounter = distribuitionDate;
                //child.Counter = 1;
                counter.UsedCounters.Add(child);

                ResponseDocumentTransaction documentTransaction = UnitOfWork.CounterRepository.Register(counter);
                //msj = msj + " - " + "Paso 5 " + documentTransaction.Message;
                return documentTransaction.IsSuccess ? Tuple.Create(registerResult.Item1, registerResult.Item2) : Tuple.Create(false, documentTransaction.Message);
            }

            ResponseTransaction responseTransaction = UnitOfWork.CounterRepository.AppendChild(counter.DocumentEntry, child);
            return responseTransaction.IsSuccess ? Tuple.Create(registerResult.Item1, registerResult.Item2) : Tuple.Create(false, responseTransaction.Message);
        }

        private OARD parse_to_sap_object(RouteBuildResponse route, DateTime distribuitionDate)
        {
            var _TransporterGeneric = UnitOfWork.SettingsRepository.Setting(OPDS.Codes.TRANSPORTER_GENERIC);
            var transporter = UnitOfWork.TransporterRepository
                .RetrieveTransporters(t => t.SupplierId == route.TransporterId)
                .FirstOrDefault();
            TRD1 driver;

            if (transporter == null)
            {
                transporter = UnitOfWork.TransporterRepository.RetrieveTransporters(t => t.SupplierId == _TransporterGeneric.Value).FirstOrDefault();
                driver = transporter.Drivers.FirstOrDefault(t => t.DriveLicense == transporter.Drivers.FirstOrDefault().DriveLicense);
                var _vehi = transporter.Vehicles.FirstOrDefault();
                route.VehicleId = _vehi.Code;
            }
            else
            {
                driver = transporter.Drivers
               .FirstOrDefault(t => t.DriveLicense == route.DriverId);
            }


            var item = new OARD();
            item.DistributionDate = distribuitionDate;
            item.CodigoTemporalReferencia = route.Id;
            item.DriverId = driver.DriveLicense;
            item.DriverName = driver.DriverName;
            item.SupplierId = transporter.SupplierId;
            item.SupplierName = transporter.SupplierName;
            item.ReferenceCounter = route.ReferenceCounter;
            item.RouteId = route.Id;
            item.VehicleId = route.VehicleId;
            item.VehicleLisencePlate = route.VehicleId;
            item.TotalVolume = route.TotalVolume.ToDouble();
            item.TotalWeight = route.TotalWeight.ToDouble();
            var startHour = route.DeliveryPointsRelated.First().StartHour.ToString("00:00").Split(':');

            item.StartHour = new DateTime().AddHours(startHour[0].ToDouble()).AddMinutes(startHour[1].ToDouble());
            try
            {
                var endHour = route.DeliveryPointsRelated.Last().LeaveHour.ToString("00:00").Split(':');
                item.EndHour = new DateTime().AddHours(endHour[0].ToDouble()).AddMinutes(endHour[1].ToDouble());
            }
            catch (Exception)
            {
            }

            item.AuxiliarCode = route.AuxiliaryId;
            item.AuxiliarCodeExtra = route.AuxiliaryIdExtra;
            item.PrioridadCarga = route.Prioridad;
            item.RelatedTransferOrders = route.DeliveryPointsRelated
                .Select(delegate (DeliveryPointsRelated pointsRelated)
                {
                    if (pointsRelated.ArriveHour > 2400)
                        pointsRelated.ArriveHour = pointsRelated.ArriveHour - 2400;
                    var arriveHour = pointsRelated.ArriveHour.ToString("00:00").Split(':');
                    var transferOrder = UnitOfWork.TransferOrderRepository
                        .Retrieve(t => t.DocumentEntry == pointsRelated.TransferOrderId)
                        .Single();

                    return new ARD1
                    {
                        TransferOrderDocEntry = pointsRelated.TransferOrderId,
                        TransferOrderNumber = transferOrder.DocumentNumber,
                        FlowSource = transferOrder.FJOR,
                        LoadType = transferOrder.TPCR,
                        ClientId = transferOrder.COCL,
                        ClientName = transferOrder.DSCL,
                        AddressId = transferOrder.COEN,
                        AddressDescription = transferOrder.DREN,
                        DeliveryHour = new DateTime().AddHours(arriveHour[0].ToDouble())
                            .AddMinutes(arriveHour[1].ToDouble()), //add to string to end
                        TotalWeight = pointsRelated.Weight.ToDouble(),
                        TotalVolume = pointsRelated.Volume.ToDouble()
                    };
                })
                .ToList();

            item.OriginFlow = item.RelatedTransferOrders.First().FlowSource;
            if (item.OriginFlow == "SE")
                item.RouteStatus = OARD.Status.PROGRAMED;

            item.Series = retrieve_route_series(item.OriginFlow);

            return item;
        }
    }
}