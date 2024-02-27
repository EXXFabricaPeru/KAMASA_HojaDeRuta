using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.Repositories;
using GeolocationAPI.Storage;
using LiteDB;

namespace GeolocationAPI.Utilities
{
    public class AssignAlgorithm
    {
        private string _key;
        private readonly string _localStoragePath;
        private IList<Route> _unassignedRoutes;
        private IList<Vehicle> _selectedVehicles;
        private IList<Transporter_Vehicle> _selectedTransporterVehicles;
        private UnitOfWork _unitOfWork;
        private IList<AssignedCertify> _Auxiliares;
        private IList<AssignedCertify> _Drivers;

        public AssignAlgorithm(string key, IList<Route> unassignedRoutes, UnitOfWork unitOfWork, string localStoragePath)
        {
            _key = key;
            _unassignedRoutes = unassignedRoutes;
            _unitOfWork = unitOfWork;
            _localStoragePath = localStoragePath;
            _selectedVehicles = new List<Vehicle>();
            _Auxiliares = new List<AssignedCertify>();
            _Drivers = new List<AssignedCertify>();
        }

        public async Task<IEnumerable<Route>> AssignTransporter()
        {
            IList<Route> result = new List<Route>();
            foreach (var unassignedRoute in _unassignedRoutes)
            {
                var assignedRoute = await assign_transporter(unassignedRoute);
                result.Add(assignedRoute);
            }

            return result;
        }

        // public async IAsyncEnumerable<Route> AssignTransporter()
        // {
        //     foreach (var unassignedRoute in _unassignedRoutes)
        //     {
        //         var assignedRoute = await assign_transporter(unassignedRoute);
        //         yield return assignedRoute;
        //     }
        // }

        private string[] get_available_plates(string key)
        {
            using var database = new LiteDatabase(_localStoragePath);
            var collection = database.GetCollection<AvailableVehicles>("available_vehicles");
            var availableVehicles = collection.FindById(new ObjectId(key));
            return availableVehicles.VehiclesIds;
        }

        private async Task<Route> assign_transporter(Route route)
        {
            IEnumerable<string> availablePlates = get_available_plates(_key);


            var availableVehicles = availablePlates.Select(async t => await _unitOfWork.VehicleRepository.RetrieveVehicle(t));
            IEnumerable<Vehicle> vehicles = await Task.WhenAll(availableVehicles);

            // vehicles = await _unitOfWork.VehicleRepository
            //     .RetrieveVehicleWithMaxLoads(route.DeliveryControlMaxWeight, route.DeliveryControlMaxVolume);
            if (route.Id == "RT-0007")
            {
                var x = "";
            }

            var availableVehicles2 = availablePlates.Select(async t => await _unitOfWork.TransporterRepository.RetrieveTransporterVehicle(t));
            IEnumerable<Transporter_Vehicle> vehicles2 = await Task.WhenAll(availableVehicles2);
            vehicles2 = vehicles2.Where(t => t.Value.Count > 0).ToList();
            vehicles = vehicles
                .Where(t => t.WeightCapacity >= route.TotalWeight && t.VolumeCapacity >= route.TotalVolume)
                .OrderBy(t => t.WeightCapacity)
                .ThenBy(t => t.VolumeCapacity)

                .ToList();
            IList<Transporter_Vehicle> vehicles3 = new List<Transporter_Vehicle>();
            //vehicles2 = vehicles2.Select(t=>t).Distinct().ToList();
            //foreach (var item in vehicles2.)
            //{

            //    foreach (var item2 in item.Vehicles)
            //    {
            //        vehicles3.Add(item2);
            //    }
            //}

            Transporter_Vehicle v3 = new Transporter_Vehicle();
            //v3 = vehicles2.FirstOrDefault();
            vehicles2 = vehicles2
                .Where(t => decimal.Parse(t.Value[0].VSPDOTRDVSPDTRD2Collection.WeightCapacity.ToString()) >= route.TotalWeight && decimal.Parse(t.Value[0].VSPDOTRDVSPDTRD2Collection.VolumeCapacity.ToString()) >= route.TotalVolume)
                .OrderByDescending(t => t.Value[0].VSPDOTRDVSPDTRD2Collection.WeightCapacity)
                .ThenBy(t => t.Value[0].VSPDOTRDVSPDTRD2Collection.VolumeCapacity)
                .ThenBy(t => t.Value[0].VSPDOTRD.Priority)
                .ToList();

            foreach (var item in vehicles2)
            {
                vehicles.Where(t => t.Code == item.Value.FirstOrDefault().VSPDOTRDVSPDTRD2Collection.Code).FirstOrDefault().Priority = int.Parse(item.Value.FirstOrDefault().VSPDOTRD.Priority);
            }
            vehicles = vehicles
                .Where(t => t.WeightCapacity >= route.TotalWeight && t.VolumeCapacity >= route.TotalVolume).
                OrderByDescending(t => t.Priority)
                .ThenBy(t => t.WeightCapacity)
                .ThenBy(t => t.VolumeCapacity)

                .ToList();

            //for (int i = 0; i < vehicles.Count(); i++)
            //{

            //}

            IList<string> clientIds = new List<string>();
            foreach (var routeDetail in route.DeliveryPointsRelated)
            {
                var transferOrder = await _unitOfWork.TransferOrderRepository.RetrieveTransferOrder(routeDetail.TransferOrderId);
                if (clientIds.All(t => t != transferOrder.ClientId))
                    clientIds.Add(transferOrder.ClientId);
            }

            IList<string> assignedCertifies = new List<string>();

            if (!clientIds.Contains("SORAYA") && !clientIds.FirstOrDefault().StartsWith("PL"))
                foreach (var clientId in clientIds)
                {
                    var assignationOfBusinessPartner = await _unitOfWork.AssignedCertifyRepository.RetrieveAssignationOfBusinessPartner(clientId);
                    foreach (var referenceCertify in assignationOfBusinessPartner.Certifies.Where(referenceCertify =>
                        assignedCertifies.All(t => t != referenceCertify.Id)))
                    {
                        assignedCertifies.Add(referenceCertify.Id);
                    }
                }

            IList<AssignedCertify> drivers = new List<AssignedCertify>();

            var driversToAssign = await _unitOfWork.AssignedCertifyRepository.RetrieveAllAssignationOfDriver();
            foreach (var driver in driversToAssign)
            {
                if (assignedCertifies.Any())
                {
                    var hasUnavailable = false;
                    foreach (var certify in driver.Certifies)
                    {
                        hasUnavailable = !assignedCertifies.Contains(certify.Id);
                        if (hasUnavailable)
                            break;
                    }

                    if (hasUnavailable == false)
                        continue;
                }

                drivers.Add(driver);
            }
            // drivers = drivers.Where(t => t.Certifies.All(x => assignedCertifies.Contains(x.Id)))
            //     .ToList();


            var transporters = await _unitOfWork.TransporterRepository.RetrieveAll();
            Transporter selectedTransporter = null;
            string vehicleId = string.Empty;
            string driverId = string.Empty;
            AssignedCertify driverBean = new AssignedCertify();
            transporters = transporters.OrderByDescending(t => t.Priority);
            foreach (var transporter in transporters)
            {
                var valid = false;
                foreach (var transporterDriver in transporter.Drivers)
                {
                    foreach (var item in _Drivers)
                    {
                        drivers = drivers.Where(t => t.ReferenceEntityId != item.ReferenceEntityId).ToList();
                    }
                    valid = drivers.Any(t => t.ReferenceEntityId == transporterDriver.ReferenceId);
                    if (valid)
                    {
                        driverId = transporterDriver.License;//ReferenceId
                        driverBean = drivers.Where(t => t.ReferenceEntityId == transporterDriver.ReferenceId).First();
                        break;
                    }
                }

                if (valid == false)
                    continue;

                valid = false;
                foreach (var transporterVehicle in transporter.Vehicles)
                {
                    var vehicle = vehicles.Except(_selectedVehicles, new VehicleComparer()).FirstOrDefault(t => t.Name == transporterVehicle.Plate);//ReferenceId
                    //var vehicle2 = vehicles2.Except(_selectedTransporterVehicles, new VehicleComparer()).FirstOrDefault(t => t.Code == transporterVehicle.ReferenceId);
                    if (vehicle != null)
                    {
                        _selectedVehicles.Add(vehicle);
                        vehicleId = transporterVehicle.Plate;//ReferenceId
                        valid = true;
                        break;
                    }
                }

                if (valid == false)
                    continue;

                selectedTransporter = transporter;
                break;
            }

            route.TransporterId = selectedTransporter?.Code;
            route.DriverId = driverId;
            _Drivers.Add(driverBean);
            route.VehicleId = vehicleId;
            return route;
        }

        public async IAsyncEnumerable<Route> AssignAuxiliary()
        {
            foreach (var item in _unassignedRoutes)
            {
                var auxiliaries = await _unitOfWork.AssignedCertifyRepository.RetrieveAllAssignationOfAuxiliaryByCode(item.DriverId);
                if (auxiliaries.Count() > 0)
                {
                    foreach (var aux in auxiliaries)
                    {
                        _Auxiliares.Add(aux);
                    }
                }

            }

            foreach (var unassignedRoute in _unassignedRoutes)
            {
                var assignedRoute = await assign_auxiliary(unassignedRoute);
                yield return assignedRoute;
            }
        }

        private async Task<Route> assign_auxiliary(Route route)
        {
            IList<string> clientIds = new List<string>();
            foreach (var routeDetail in route.DeliveryPointsRelated)
            {
                var transferOrder = await _unitOfWork.TransferOrderRepository.RetrieveTransferOrder(routeDetail.TransferOrderId);
                if (clientIds.All(t => t != transferOrder.ClientId))
                    clientIds.Add(transferOrder.ClientId);
            }

            IList<string> assignedCertifies = new List<string>();

            if (!clientIds.Contains("SORAYA") && !clientIds.FirstOrDefault().StartsWith("PL"))
                foreach (var clientId in clientIds)
                {
                    var assignationOfBusinessPartner = await _unitOfWork.AssignedCertifyRepository.RetrieveAssignationOfBusinessPartner(clientId);
                    foreach (var referenceCertify in assignationOfBusinessPartner.Certifies.Where(referenceCertify =>
                        assignedCertifies.All(t => t != referenceCertify.Id)))
                    {
                        assignedCertifies.Add(referenceCertify.Id);
                    }
                }

            var auxiliaries = await _unitOfWork.AssignedCertifyRepository.RetrieveAllAssignationOfAuxiliary();
            //auxi
            auxiliaries = auxiliaries.Where(t => filter_auxiliary(t, assignedCertifies));


            foreach (var item in _Auxiliares)
            {
                auxiliaries = auxiliaries.Where(t => t.ReferenceEntityId != item.ReferenceEntityId);
            }
            //var aux2 = auxiliaries.Where (t=> t!= _Auxiliares).ToList();
            try
            {
                route.AuxiliaryId = auxiliaries.First(t=>t.ReferenceEntityId != route.DriverId).ReferenceEntityId;
                _Auxiliares.Add(auxiliaries.First(t => t.ReferenceEntityId != route.DriverId));
            }
            catch (System.Exception)
            {

                throw new Exception("No hay auxiliares para la ruta: "+ route.Id);
            }
            
            
            return route;
        }

        private bool filter_auxiliary(AssignedCertify auxiliary, IEnumerable<string> certifies)
        {
            if (!certifies.Any())
                return true;

            return certifies.All(t => auxiliary.Certifies.Any(certify => certify.Id == t));
        }
    }
}