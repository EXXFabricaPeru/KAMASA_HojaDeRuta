using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.Repositories;
using GeolocationAPI.Request;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Storage;
using GeolocationAPI.Utilities;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace GeolocationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UnitOfWork _unitOfWork;
        private readonly string _mapboxToken;

        private string local_storage_path => $"{_webHostEnvironment.ContentRootPath}\\api_storage.db";

        public RouteController(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, string mapboxToken)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapboxToken = mapboxToken;
        }

        [HttpPost("register-vehicles")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RegisterVehicles([FromBody] RegisterVehiclesRequest request)
        {
            using var database = new LiteDatabase(local_storage_path);
            var collection = database.GetCollection<AvailableVehicles>("available_vehicles");
            collection.UpdateMany(vehicle => new AvailableVehicles
            {
                Id = vehicle.Id,
                VehiclesIds = vehicle.VehiclesIds,
                IsActive = false
            }, vehicle => vehicle.IsActive);

            var availableVehicles = new AvailableVehicles {Id = ObjectId.NewObjectId(), VehiclesIds = request.PlateIds, IsActive = true};
            var value = collection.Insert(availableVehicles);
            return StatusCode(StatusCodes.Status201Created, value.AsObjectId.ToString());
        }

        [HttpPost("build")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Build([FromBody] string key)
        {
            try
            {
                var routeMapRepository = _unitOfWork.RouteMapRepository;
                var algorithm = new RouteAlgorithm(key, _mapboxToken, _unitOfWork, local_storage_path);
                await algorithm.Initialize();
                var routes = await algorithm.StartProcess().ToListAsync();
                return StatusCode(StatusCodes.Status201Created, routes);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
            }
        }

        [HttpPost("calculate-distance")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateDistance()
        {
            try
            {
                var availableVehicles = (await _unitOfWork.VehicleRepository.RetrieveAvailableVehicle())
               .Where(t => t.IsAvailable == "Y");

                var registerVehiclesResult = RegisterVehicles(new RegisterVehiclesRequest { PlateIds = availableVehicles.Select(t => t.PlateID).ToArray() });
                var vehiclesResult = (ObjectResult)registerVehiclesResult;
                var buildRoutesResult = await Build(vehiclesResult.Value?.ToString() ?? string.Empty);
                await buildRoutesResult.ExecuteResultAsync(ControllerContext);
                var sta= StatusCode(StatusCodes.Status200OK); 
                return sta;
            }
            catch (Exception)
            {
                return null;
            }
           
        }

        [HttpPost("assign-transporter")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignTransporter([FromBody] AssignRequest request)
        {
            try
            {
                var algorithm = new AssignAlgorithm(request.Key, request.Routes, _unitOfWork, local_storage_path);
                var routes = await algorithm.AssignTransporter();
                return StatusCode(StatusCodes.Status200OK, routes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            
           
;        }

        [HttpPost("assign-auxiliary")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignAuxiliary([FromBody] AssignRequest request)
        {
            try
            {
                var algorithm = new AssignAlgorithm(request.Key, request.Routes, _unitOfWork, local_storage_path);
                var routes = await algorithm.AssignAuxiliary().ToListAsync();
                return StatusCode(StatusCodes.Status200OK, routes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
         
        }
    }
}