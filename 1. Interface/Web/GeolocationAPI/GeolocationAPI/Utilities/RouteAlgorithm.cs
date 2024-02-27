using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonUtilities.Extensions;
using GeolocationAPI.Code;
using GeolocationAPI.Models;
using GeolocationAPI.Repositories;
using GeolocationAPI.Storage;
using JetBrains.Annotations;
using LiteDB;

namespace GeolocationAPI.Utilities
{
    public class RouteAlgorithm
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly string _key;
        private readonly string _localStoragePath;
        private readonly string _mapboxKey;

        private IEnumerable<TransferOrder> _transferOrders;
        private IEnumerable<Velocity> _velocities;
        private IEnumerable<Configuration> _configurations;
        private IEnumerable<Vehicle> _vehicles;
        private List<TimeWindowDispatchPerOrder> _timeWindowsDispatches;
        private List<TimeWindowDispatchPerOrder> _timeWindowsDispatchesBack;
        private IEnumerable<TimeDispatch> _timeDispatches;
        private IEnumerable<DeliveryControl> _deliveryControls;
        private int _startHour;
        private decimal _maxWeight;
        private decimal _maxVolume;
        private decimal _maxFreezeWeight;
        private decimal _maxFreezeVolume;
        private decimal _optimizationTimePriorityIndicator;
        private decimal _timeWindowPriorityIndicator;
        private decimal _startTimePriorityIndicator;
        private decimal _leaveTimePriorityIndicator;
        private decimal _weightPriorityIndicator;
        private decimal _volumePriorityIndicator;
        private decimal _weightFreezePriorityIndicator;
        private decimal _volumeFreezePriorityIndicator;
        private decimal _distancePriorityIndicator;

        public RouteAlgorithm(string key, string mapboxKey, UnitOfWork unitOfWork, string localStoragePath)
        {
            _key = key;
            _unitOfWork = unitOfWork;
            _localStoragePath = localStoragePath;
            _mapboxKey = mapboxKey;
        }

        public async Task Initialize()
        {
            var priorityIndicators = await _unitOfWork.PriorityIndicatorRepository.PriorityIndicators();
            _timeWindowPriorityIndicator = get_indicator_value(priorityIndicators, "VNT_HOR");
            _leaveTimePriorityIndicator = get_indicator_value(priorityIndicators, "TMP_SAL");
            _startTimePriorityIndicator = get_indicator_value(priorityIndicators, "TMP_PAR");
            _optimizationTimePriorityIndicator = get_indicator_value(priorityIndicators, "TMP_OPT");
            _weightPriorityIndicator = get_indicator_value(priorityIndicators, "PES_DSP");
            _volumePriorityIndicator = get_indicator_value(priorityIndicators, "VOL_DSP");
            _weightFreezePriorityIndicator = get_indicator_value(priorityIndicators, "PES_FRI");
            _volumeFreezePriorityIndicator = get_indicator_value(priorityIndicators, "VOL_FRI");
            _distancePriorityIndicator = get_indicator_value(priorityIndicators, "DIS_REC");
            _velocities = await _unitOfWork.VelocityRepository.RetrieveVelocities();
            _configurations = await _unitOfWork.ConfigurationRepository.RetrieveConfigurations();
            _startHour = await get_start_hour();
            _vehicles = await get_vehicles(_key).ToListAsync();
            _timeDispatches = await _unitOfWork.TimeDispatchRepository.RetrieveTimeDispatches();
            _deliveryControls = await _unitOfWork.DeliveryControlRepository.RetrieveDeliveryControls();
            _transferOrders = (await get_transfer_orders()).ToList();

            _timeWindowsDispatches = await join_time_window_with_translate_order().ToListAsync();
            _timeWindowsDispatchesBack = _timeWindowsDispatches;

            if (_transferOrders.Any())
            {
                _maxWeight = _transferOrders.Max(t => t.TotalWeight);
                _maxVolume = _transferOrders.Max(t => t.TotalVolume);
                _maxFreezeWeight = _transferOrders.Max(t => t.TotalFreezeWeight ?? 0);
                _maxFreezeVolume = _transferOrders.Max(t => t.TotalFreezeVolume ?? 0);
            }


        }

        private decimal get_indicator_value([NoEnumeration] IEnumerable<PriorityIndicator> indicators, string code)
        {
            PriorityIndicator priorityIndicator = indicators.SingleOrRiseException(Error.IndicatorNotDefined(code), t => t.Code == code);
            return priorityIndicator.Value * Convert.ToInt32(priorityIndicator.Type);
        }

        private async Task<int> get_start_hour()
        {
            var startHour = await _unitOfWork.SettingRepository.RetrieveStartHour();
            return HourHelper.ParseToHour(startHour.Value);
        }

        private async Task<IEnumerable<TransferOrder>> get_transfer_orders()
        {
            IEnumerable<TransferOrder> transferOrders = await _unitOfWork.TransferOrderRepository.OpenTransferOrders();
            transferOrders = transferOrders
               .Select(assign_dispatch_time)
                .ToList();

            foreach (var item in transferOrders)
            {
                if (item.OriginFlow == "SA")
                {
                    var order = transferOrders.Where(t => t.DeliveryAddress == item.DeliveryAddress && t.OriginFlow == "RE").FirstOrDefault();
                    if (order != null)
                        item.DocEntryReturn = order.DocEntry;

                }
                if (string.IsNullOrEmpty(item.DeliveryControl))
                {
                    throw new Exception("La OT no tiene restricción de entrega asignada - " + item.DocNum);
                }
                if (_deliveryControls.Where(t => t.Code == item.DeliveryControl).FirstOrDefault().WeightLoad < item.TotalWeight)
                {
                    throw new Exception("La restricción de entrega asignada tiene un peso menor al peso total de la OT " + item.DocNum);
                }

            }

            return transferOrders;
            TransferOrder assign_dispatch_time(TransferOrder transferOrder)
            {
                if (transferOrder.OriginFlow == "RE")
                {
                    transferOrder.TotalWeight = 0.00M;
                    transferOrder.TotalVolume = 0.00M;
                    transferOrder.TotalFreezeVolume = 0.00M;
                    transferOrder.TotalFreezeWeight = 0.00M;
                }
                if (transferOrder.OriginFlow == "TR")
                {
                    if (transferOrder.DispatchTime == null)
                    {
                        throw new Exception("Los traslados deben tener un tiempo de despacho asignado");
                    }
                }
                if (transferOrder.DispatchTime != null)
                    return transferOrder;

                if (string.IsNullOrEmpty(transferOrder.SaleChannel))
                    throw Error.SaleChannelNotAssigned(transferOrder.DocNum);

                TimeDispatch timeDispatch = _timeDispatches
                    .SingleOrRiseException(Error.SaleChannelNotReferencedDispatchTime(transferOrder.SaleChannel),
                    dispatch => dispatch.SaleChannel == transferOrder.SaleChannel);
                transferOrder.DispatchTime = timeDispatch.Duration;
                // TO DO

                return transferOrder;
            }
        }

        private async IAsyncEnumerable<Vehicle> get_vehicles(string key)
        {
            IEnumerable<string> availablePlates = get_available_plates(key);
            foreach (string availablePlate in availablePlates)
            {
                Vehicle vehicle = await _unitOfWork.VehicleRepository.RetrieveVehicle(availablePlate);
                yield return vehicle;
            }
        }

        private string[] get_available_plates(string key)
        {
            using var database = new LiteDatabase(_localStoragePath);
            var collection = database.GetCollection<AvailableVehicles>("available_vehicles");
            var availableVehicles = collection.FindById(new ObjectId(key));
            return availableVehicles.VehiclesIds;
        }

        private async IAsyncEnumerable<TimeWindowDispatchPerOrder> join_time_window_with_translate_order()
        {
            var timeWindowDispatches = await get_time_windows().ToListAsync();
            foreach (var transferOrder in _transferOrders)
            {
                TimeWindowDispatchPerOrder T = new TimeWindowDispatchPerOrder();
                T.DocNum = transferOrder.DocNum;
                T.TimeWindowDispatch = timeWindowDispatches.Single(t => t.Code == transferOrder.TimeWindowDispatch);
                T.TimeWindowDispatch.Identificador = T.TimeWindowDispatch.Code;
                T.TimeWindowDispatch.Code = T.DocNum.ToString() + "-" + T.TimeWindowDispatch.Identificador;
                timeWindowDispatches = await get_time_windows().ToListAsync();
                yield return T;
                //yield return new TimeWindowDispatchPerOrder
                //{.
                //    DocNum = transferOrder.DocNum,
                //    TimeWindowDispatch = timeWindowDispatches.Single(t => t.Code == transferOrder.TimeWindowDispatch),

                //};
            }
        }

        private async IAsyncEnumerable<TimeWindowDispatch> get_time_windows()
        {
            foreach (TransferOrder transferOrder in _transferOrders)
            {
                if (string.IsNullOrEmpty(transferOrder.TimeWindowDispatch))
                    throw Error.DispatchTimeNotAssigned(transferOrder.DocNum);

                if (string.IsNullOrEmpty(transferOrder.DeliveryControl))
                    throw Error.DeliveryControlNotAssigned(transferOrder.DocNum);
            }

            var codes = _transferOrders.DistinctElements(t => t.TimeWindowDispatch);
            foreach (var code in codes)
            {
                int cont = 1;

                var timeWindow = await _unitOfWork.TimeWindowDispatchRepository.RetrieveTimeWindowDispatch(code);
                try
                {
                    timeWindow.RelatedHours.First(t => t.LineId == cont).IsUsed = true;
                    //timeWindow.RelatedHours.First(t => t.LineId == 1).IsUsed = true;
                }
                catch (Exception)
                {
                    cont = timeWindow.RelatedHours.OrderBy(t => t.LineId).First().LineId;
                    timeWindow.RelatedHours.First(t => t.LineId == cont).IsUsed = true;
                }



                yield return timeWindow;
            }
        }

        public async IAsyncEnumerable<Route> StartProcess()
        {
            while (_transferOrders.Any(t => t.IsAssigned == false))
            {
                var route = new Route
                {
                    Id = string.Empty,
                    Create = DateTime.Now,
                    DeliveryControlMaxWeight = decimal.MaxValue,
                    DeliveryControlMaxVolume = decimal.MaxValue
                };
                route = await make_route(route);
                restart_time_windows_dispatches();
                yield return route;
            }
        }

        private async Task<Route> make_route(Route actualRoute, string startNode = null, int startHour = 0)
        {
            var indicators = await calculate_indicators(startNode, startHour,
                actualRoute.DeliveryPointsRelated.Count > 0 ? actualRoute.DeliveryPointsRelated.Last().DeliveryControl : "");



            // indicators = indicators.OrderBy(t => t.TotalIndicatorValue)
            //     .ToList();
            if (!string.IsNullOrEmpty(startNode))
            {
                var _orderRef = _transferOrders.Single(t => t.DocEntry == Convert.ToInt32(startNode));
                if (_orderRef.DocEntryReturn != null)
                {
                    var _orderRefReturn = _transferOrders.
                                            FirstOrDefault(t => t.DocEntry == _orderRef.DocEntryReturn && t.IsAssigned == false);

                    if (_orderRefReturn != null)
                        indicators = indicators.Where(t => t.Id == _orderRef.DocEntryReturn);
                    else
                        indicators = indicators.OrderBy(t => t.TotalIndicatorValue);
                }

                else
                    indicators = indicators.OrderBy(t => t.TotalIndicatorValue)
               .Where(t => t.IsPossibleToDispatch).ToList();
            }
            else
            {
                indicators = indicators.OrderBy(t => t.TotalIndicatorValue)
             .Where(t => t.IsPossibleToDispatch).ToList();
            }


            foreach (var indicator in indicators)
            {
                var transferOrder = _transferOrders.Single(t => t.DocEntry == indicator.Id);
                var deliveryControl = _deliveryControls.Single(t => t.Code == transferOrder.DeliveryControl);
                if (actualRoute.DeliveryControlMaxVolume > actualRoute.TotalVolume + transferOrder.TotalVolume &&
                    actualRoute.DeliveryControlMaxWeight > actualRoute.TotalWeight + transferOrder.TotalWeight &&
                    indicator.IsPossibleToDispatch)
                {
                    if (actualRoute.DeliveryControlMaxWeight > deliveryControl.WeightLoad)
                    {
                        if (deliveryControl.VolumeLoad < actualRoute.TotalVolume + transferOrder.TotalVolume ||
                            deliveryControl.WeightLoad < actualRoute.TotalWeight + transferOrder.TotalWeight)
                        {
                            indicator.IsPossibleToDispatch = false;
                            continue;
                        }

                        actualRoute.DeliveryControlMaxWeight = deliveryControl.WeightLoad;
                        actualRoute.DeliveryControlMaxVolume = deliveryControl.VolumeLoad;
                    }

                    actualRoute.DeliveryPointsRelated.Add(new RouteDetail
                    {
                        TransferOrderId = transferOrder.DocEntry,
                        Distance = indicator.Distance,
                        TimeWindow = transferOrder.TimeWindowDispatch,
                        StartHour = indicator.StartTime,
                        TravelTime = indicator.TravelTime,
                        ArriveHour = indicator.ArriveTime2 == 0 ? indicator.ArriveTime : indicator.ArriveTime2,
                        LeaveHour = indicator.ArriveTime2 == 0 ? indicator.LeaveTime : indicator.LeaveTime2,
                        DeliveryControl = transferOrder.DeliveryControl, // + "-" + indicator.TotalIndicatorValue.ToString("#.##"),
                        Weight = transferOrder.TotalWeight,
                        Volume = transferOrder.TotalVolume,
                        FreezeWeight = transferOrder.TotalFreezeWeight ?? 0,
                        FreezeVolume = transferOrder.TotalFreezeVolume ?? 0,
                    });
                    transferOrder.IsAssigned = true;
                    if (transferOrder.OriginFlow == "SE")
                    {
                        return actualRoute;
                    }
                    var buildRoute = await make_route(actualRoute, transferOrder.DocEntry.ToString(),
                        indicator.ArriveTime2 == 0 ? indicator.LeaveTime : indicator.LeaveTime2);

                    if (!string.IsNullOrEmpty(startNode))
                        return buildRoute;

                    var order = _transferOrders.First(t => t.DocEntry == buildRoute.DeliveryPointsRelated[0].TransferOrderId);
                    if (order.OriginFlow == "TR")
                    {
                        buildRoute.LeaveAddressId = order.LeaveAddressId;
                    }
                    else
                    {
                        var mainWarehouse = await _unitOfWork.WarehouseRepository.RetrieveMainWarehouse();
                        buildRoute.LeaveAddressId = mainWarehouse.Code;
                    }

                    return buildRoute;
                }

                indicator.IsPossibleToDispatch = false;
            }

            if (!indicators.Any())
                return actualRoute;

            if (!indicators.Any(t => t.IsPossibleToDispatch))
                return actualRoute;

            if (indicators.Count() == 16)
            {
                return actualRoute;
            }

            if (_timeWindowsDispatches.Any(t => t.TimeWindowDispatch.RelatedHours.Any(rh => rh.IsUsed)))
                return await make_route(actualRoute, startNode, startHour);

            return actualRoute;
        }

        private async Task<MapRoute> get_map_route(string startNode)
        {
            if (startNode == "49")
            {
                var x = 0;
            }
            var mainWarehouse = await _unitOfWork.WarehouseRepository.RetrieveMainWarehouse();
            var transferOrder = string.IsNullOrEmpty(startNode)
                ? _transferOrders.First(t => t.IsAssigned == false)
                : _transferOrders.Single(t => t.DocEntry == Convert.ToInt32(startNode));

            var deliveryAddressId = string.IsNullOrEmpty(startNode)
                ? mainWarehouse.Code
                : transferOrder.DeliveryAddressId;

            var clientId = string.IsNullOrEmpty(startNode) ? "SORAYA" : transferOrder.ClientId;

            var mapRoute = await _unitOfWork.RouteMapRepository.RetrieveMapRoute(clientId, deliveryAddressId);

            if (mapRoute == null)
            {
                if (transferOrder.OriginFlow == "TR")
                {
                    var warehouse = await _unitOfWork.WarehouseRepository.RetrieveWarehouse(deliveryAddressId);
                    mapRoute = new MapRoute
                    {
                        ClientId = "SORAYA",
                        ClientName = "SORAYA",
                        AddressId = warehouse.Code,
                        AddressContent = warehouse.Name,
                        AddressLatitude = warehouse.Latitude,
                        AddressLongitude = warehouse.Longitude
                    };
                }
                else
                {
                    if (string.IsNullOrEmpty(startNode))
                    {
                        if (clientId == "CL20298674611")
                        {
                            mainWarehouse.Code = string.IsNullOrEmpty(mainWarehouse.Code) ? mainWarehouse.Code : mainWarehouse.Code.Replace("'", "%5C%22%22");
                        }
                        mapRoute = new MapRoute
                        {
                            ClientId = clientId,
                            ClientName = clientId,
                            AddressId = mainWarehouse.Code,
                            AddressContent = mainWarehouse.Name,
                            AddressLatitude = mainWarehouse.Latitude,
                            AddressLongitude = mainWarehouse.Longitude
                        };
                    }
                    else
                    {
                        var businessPartner = await _unitOfWork.BusinessPartnerRepository.RetrieveBusinessPartner(clientId);
                        var address = businessPartner.BusinessPartnerAddresses
                            .SingleOrRiseException(Error.BusinessPartnerNotReferencedShipAddress(clientId),
                                t => t.AddressName == deliveryAddressId && t.AddressType == "bo_ShipTo");

                        if (clientId == "CL20298674611")
                        {
                            //address.AddressName = string.IsNullOrEmpty(address.AddressName) ? address.AddressName : address.AddressName.Replace("'", "");
                        }
                        mapRoute = new MapRoute
                        {
                            ClientId = businessPartner.CardCode,
                            ClientName = businessPartner.CardName,
                            AddressId = address.AddressName,
                            AddressContent = address.Street,
                            AddressLatitude = address.Latitude ?? 0,
                            AddressLongitude = address.Longitude ?? 0
                        };
                    }
                }

                var documentEntry = await _unitOfWork.RouteMapRepository.InsertMapRoute(mapRoute);
                mapRoute.DocEntry = documentEntry;
                mapRoute.AddressDistanceRelated = new List<MapRouteAddressDistanceRelated>();
            }

            mapRoute.OriginFlow = transferOrder.OriginFlow;

            /*mapRoute.OriginFlow = transferOrder.OriginFlow == "RE" ? "SA" : transferOrder.OriginFlow;*/
            return mapRoute;
        }

        private async Task<IEnumerable<TransferOrderIndicator>> calculate_indicators(string startNode, int startHour, string rest)
        {
            IList<TransferOrderIndicator> result = new List<TransferOrderIndicator>();
            _timeWindowsDispatches = _timeWindowsDispatchesBack;

            MapRoute mapRoute = await get_map_route(startNode);

            //var filtrableTransferOrders = _transferOrders
            //              .Where(t => t.IsAssigned == false && t.OriginFlow == mapRoute.OriginFlow)
            //              .ToList();
            var filtrableTransferOrders = new List<TransferOrder>();
            if (mapRoute.OriginFlow == "SA" || mapRoute.OriginFlow == "RE")
            {
                filtrableTransferOrders = _transferOrders
                               .Where(t => t.IsAssigned == false && (t.OriginFlow == "SA" || t.OriginFlow == "RE"))
                               .ToList();
                filtrableTransferOrders = filtrableTransferOrders.OrderByDescending(t => t.OriginFlow).ToList();

            }
            else
            {
                filtrableTransferOrders = _transferOrders
                         .Where(t => t.IsAssigned == false && t.OriginFlow == mapRoute.OriginFlow)
                         .ToList();
            }

            if (filtrableTransferOrders.Count<2)
            {
                var x = "";
            }
            for (var index = 0; index < filtrableTransferOrders.Count; index++)
            {

                var transferOrder = filtrableTransferOrders[index];
                try
                {

                    Velocity velocity;
                    if (startHour == default)
                    {
                        velocity = _velocities
                            .OrderBy(t => startHour)
                            .First();
                    }
                    else
                    {
                        velocity = _velocities.SingleOrRiseException(Error.VelocitiesNotDefined($"Hora Inicio <= {startHour} AND Hora Fin > {startHour} {transferOrder.DocNum}"),
                            t => t.StartHour <= startHour && t.EndHour > startHour);
                    }

                    var windowDispatch = _timeWindowsDispatches.Single(t => t.DocNum == transferOrder.DocNum);

                    var dispatchRelatedHour = windowDispatch.TimeWindowDispatch.RelatedHours.SingleOrDefault(t => t.IsUsed);

                    if (dispatchRelatedHour != null)
                    {
                        if ((int.Parse(dispatchRelatedHour.EndHour.ToString()) < startHour) && startHour != 0)
                        {
                            dispatchRelatedHour =
                                windowDispatch.TimeWindowDispatch.RelatedHours.SingleOrDefault(t =>
                                    startHour >= int.Parse(t.StarHour.ToString()) && startHour <= int.Parse(t.EndHour.ToString()));
                        }
                    }

                    //addressId = string.IsNullOrEmpty(addressId) ? addressId : addressId.Replace("\"", "%20");
                    var distanceRelated = mapRoute.AddressDistanceRelated
                        .FirstOrDefault(t => t.ClientId == transferOrder.ClientId && t.AddressId == transferOrder.DeliveryAddressId);

                    if (distanceRelated == null)
                    {
                        var t = transferOrder.ClientId;
                        var s = transferOrder.DeliveryAddressId;

                        if (transferOrder.OriginFlow == "TR")
                        {
                            var warehouse = await _unitOfWork.WarehouseRepository.RetrieveWarehouse(transferOrder.DeliveryAddressId);
                            distanceRelated = new MapRouteAddressDistanceRelated
                            {
                                ClientId = transferOrder.ClientId,
                                ClientName = transferOrder.ClientName,
                                AddressId = transferOrder.DeliveryAddressId,
                                AddressContent = warehouse.Name,
                                AddressLatitude = warehouse.Latitude,
                                AddressLongitude = warehouse.Longitude
                            };
                        }
                        else
                        {
                            var businessPartner = await _unitOfWork.BusinessPartnerRepository.RetrieveBusinessPartner(transferOrder.ClientId);
                            var address = businessPartner.BusinessPartnerAddresses
                                .SingleOrRiseException(Error.BusinessPartnerNotReferencedShipAddress(transferOrder.ClientId)
                                    , t => t.AddressName == transferOrder.DeliveryAddressId && t.AddressType == "bo_ShipTo");
                            distanceRelated = new MapRouteAddressDistanceRelated
                            {
                                ClientId = businessPartner.CardCode,
                                ClientName = businessPartner.CardName,
                                AddressId = address.AddressName,
                                AddressContent = address.Street,
                                AddressLatitude = address.Latitude ?? 0,
                                AddressLongitude = address.Longitude ?? 0
                            };
                        }

                        mapRoute.AddressDistanceRelated.Add(distanceRelated);
                    }

                    if (distanceRelated.AddressDistance == null)
                    {
                        var t = transferOrder.ClientId;
                        var s = transferOrder.DeliveryAddressId;
                        try
                        {
                            var mapboxRequest = await RestClientHelper.ExecuteDirectionsMapboxRequest(_mapboxKey,
                         ((double)mapRoute.AddressLatitude, (double)mapRoute.AddressLongitude),
                         ((double)distanceRelated.AddressLatitude, (double)distanceRelated.AddressLongitude), transferOrder);
                            var content = mapboxRequest.ContentCollectionToSpecific<DirectionsMapBox>();
                            var distanceInKilometers = content.Routes.First().Distance / 1000;
                            distanceRelated.AddressDistance = distanceInKilometers;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("VAL:  ERROR revisar los puntos de latitud y longitud de la  dirección del cliente " + transferOrder.ClientName + " - " + transferOrder.DeliveryAddressId + " Error: " + ex.Message);
                        }



                        await _unitOfWork.RouteMapRepository.UpdateMapRoute(new MapRoute
                        {
                            DocEntry = mapRoute.DocEntry,
                            AddressDistanceRelated = new List<MapRouteAddressDistanceRelated>
                            {
                                new MapRouteAddressDistanceRelated
                                {
                                    LineId = distanceRelated.LineId,
                                    AddressDistance = distanceRelated.AddressDistance,
                                    ClientId = distanceRelated.ClientId,
                                    ClientName = distanceRelated.ClientName,
                                    AddressId = distanceRelated.AddressId,
                                    AddressContent = distanceRelated.AddressContent,
                                    AddressLatitude = distanceRelated.AddressLatitude,
                                    AddressLongitude = distanceRelated.AddressLongitude
                                }
                            }
                        });

                        mapRoute = await _unitOfWork.RouteMapRepository.RetrieveMapRoute(mapRoute.ClientId, mapRoute.AddressId);
                    }


                    var indicator = new TransferOrderIndicator
                    {
                        Id = transferOrder.DocEntry,
                        DeliveryRestriction = transferOrder.DeliveryControl,
                        TimeWindowId = transferOrder.TimeWindowDispatch,
                        TimeWindow = dispatchRelatedHour != null ? int.Parse(dispatchRelatedHour.StarHour.ToString()) : int.MaxValue,
                        //TimeWindow = dispatchRelatedHour?.EndHour ?? int.MaxValue,
                        Distance = distanceRelated.AddressDistance ?? 0,
                        DispatchTime = transferOrder.DispatchTime ?? 0
                    };

                    if (dispatchRelatedHour == null)
                    {
                        indicator.IsPossibleToDispatch = false;
                        result.Add(indicator);
                        continue;
                    }

                    indicator.Weight = transferOrder.TotalWeight == 0M ? 0M : transferOrder.TotalWeight / _maxWeight;
                    indicator.Volume = transferOrder.TotalVolume == 0M ? 0M : transferOrder.TotalVolume / _maxVolume;
                    indicator.FreezeWeight = (transferOrder.TotalFreezeWeight ?? 0M) == 0M || _maxFreezeWeight == 0M
                        ? 0M
                        : transferOrder.TotalFreezeWeight.Value / _maxFreezeWeight;
                    indicator.FreezeVolume = (transferOrder.TotalFreezeVolume ?? 0M) == 0M || _maxFreezeVolume == 0M
                        ? 0M
                        : transferOrder.TotalFreezeVolume.Value / _maxFreezeVolume;



                    indicator.TravelTime = HourHelper.ConvertToHourFormat(indicator.Distance * 60 / velocity.AverageVelocity * decimal.Parse("1.3"));
                    if (transferOrder.DocNum == 1000362 || transferOrder.DocNum == 1000399) //398 //40600
                    {
                        var x = indicator.TotalIndicatorValue;
                    }

                    indicator.StartTime = startHour.IfDefault(value =>
                    {
                        var indicatorStartTime = HourHelper.SubtractHour(int.Parse(dispatchRelatedHour.StarHour.ToString()), indicator.TravelTime);
                        return indicatorStartTime >= _startHour ? indicatorStartTime : _startHour;
                    });


                    //indicator.OptimizationTime = dispatchRelatedHour.EndHour - (string.IsNullOrEmpty(startNode) ? 0 : indicator.ArriveTime); se cambio por Soraya
                    indicator.OptimizationTime = int.Parse(dispatchRelatedHour.EndHour.ToString()) - (string.IsNullOrEmpty(startNode) ? 0 : indicator.StartTime);

                    var lt = false;
                    if (indicator.ArriveTime < indicator.TimeWindow)
                    {
                        indicator.ArriveTime2 = indicator.TimeWindow;
                        lt = true;
                    }

                    var LeaveTime = lt ? indicator.LeaveTime2 : indicator.LeaveTime;
                    indicator.TotalIndicatorValue = //indicator.TimeWindow * _timeWindowPriorityIndicator +
                        indicator.OptimizationTime * _timeWindowPriorityIndicator +
                        indicator.StartTime * _startTimePriorityIndicator +
                        //indicator.LeaveTime * _leaveTimePriorityIndicator +
                        LeaveTime * _leaveTimePriorityIndicator +
                        indicator.Weight * _weightPriorityIndicator +
                        indicator.Volume * _volumePriorityIndicator +
                        indicator.FreezeWeight * _weightFreezePriorityIndicator +
                        indicator.FreezeVolume * _volumeFreezePriorityIndicator +
                        indicator.Distance * _distancePriorityIndicator;

                    var _prioridadValor = int.Parse(_configurations.Where(x => x.Code == "RUT_PR").FirstOrDefault().Valor);
                    switch (transferOrder.DispatchPriority)
                    {
                        case "BJ":
                            indicator.TotalIndicatorValue = indicator.TotalIndicatorValue + _prioridadValor;
                            break;
                        case "AL":
                            indicator.TotalIndicatorValue = indicator.TotalIndicatorValue - _prioridadValor;
                            break;
                        default:
                            break;
                    }
                    if (transferOrder.OriginFlow == "RE")
                    {
                        indicator.TotalIndicatorValue = indicator.TotalIndicatorValue + 2000;
                    }
                    var val1 = indicator.OptimizationTime * _timeWindowPriorityIndicator;
                    var val2 = indicator.StartTime * _startTimePriorityIndicator;
                    var val3 = LeaveTime * _leaveTimePriorityIndicator;
                    var val4 = indicator.Weight * _weightPriorityIndicator;
                    var val5 = indicator.Volume * _volumePriorityIndicator;
                    var val6 = indicator.FreezeWeight * _weightFreezePriorityIndicator;
                    var val7 = indicator.FreezeVolume * _volumeFreezePriorityIndicator;
                    var val8 = indicator.Distance * _distancePriorityIndicator;

                    if (indicator.Distance == decimal.Parse("18.500945") || indicator.Distance == decimal.Parse("24.023232") ||
                        indicator.Distance == decimal.Parse("7.71525") || indicator.Distance == decimal.Parse("13.539204")) //398 //40600
                    {
                        var z = transferOrder.DocNum;
                        var y = indicator.Distance;
                        var x = indicator.TotalIndicatorValue;
                    }


                    if (indicator.ArriveTime > int.Parse(dispatchRelatedHour.EndHour.ToString()))
                    {
                        if (transferOrder.TimeWindowDispatch == "V035" || transferOrder.DocNum == 1000350) //398 //40600
                        {
                            var x = "";
                        }

                        //dispatchRelatedHour.IsUsed = false;
                        //if (transferOrder.TimeWindowDispatch == "V035" || transferOrder.DocNum == 1000350)//398 //40600
                        //{
                        //    var x = "";
                        //    dispatchRelatedHour.IsUsed = true;
                        //}
                        _timeWindowsDispatches.Where(t => t.DocNum == transferOrder.DocNum).FirstOrDefault().TimeWindowDispatch.RelatedHours
                            .Single(t => t.LineId == dispatchRelatedHour.LineId).IsUsed = false;

                        var single = windowDispatch
                            .TimeWindowDispatch
                            .RelatedHours
                            .SingleOrDefault(t => t.LineId == dispatchRelatedHour.LineId + 1);
                        //if (single == null)
                        //{
                        //    single = windowDispatch
                        //    .TimeWindowDispatch
                        //    .RelatedHours
                        //    .SingleOrDefault(t => t.LineId == dispatchRelatedHour.LineId );
                        //}
                        if (single != null) single.IsUsed = true;
                        indicator.IsPossibleToDispatch = false;
                    }
                    else
                    {
                        indicator.IsPossibleToDispatch = true;
                    }
                    //if (rest != "")
                    //{
                    //    if (indicator.DeliveryRestriction != rest)
                    //    {
                    //        indicator.IsPossibleToDispatch = false;
                    //    }
                    //}

                    result.Add(indicator);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    var x = transferOrder;
                    if (exception.Message.StartsWith("VAL"))
                    {
                        throw new Exception(exception.Message);
                    }

                }
            }

            return result;
        }

        private void restart_time_windows_dispatches()
        {
            foreach (var transferOrder in _transferOrders.Where(t => t.IsAssigned == false))
            {
                var dispatch = _timeWindowsDispatches.Single(t => t.DocNum == transferOrder.DocNum);
                var relatedHours = dispatch.TimeWindowDispatch
                    .RelatedHours
                    .OrderBy(t => t.LineId)
                    .ToList();

                for (var i = 0; i < relatedHours.Count; i++)
                {
                    var relatedHour = relatedHours[i];
                    relatedHour.IsUsed = i == 0;
                }
            }
        }
    }
}