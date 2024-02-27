using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using GeolocationAPI.Repositories;
using GeolocationAPI.Request;
using GeolocationAPI.Storage;
using LiteDB;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeolocationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UnitOfWork _unitOfWork;

        private string local_storage_path => $"{_webHostEnvironment.ContentRootPath}\\api_storage.db";

        public MapController(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RegisterRoutes([FromBody] RouteRequest[] request)
        {
            using var database = new LiteDatabase(local_storage_path);
            var collection = database.GetCollection<Routes>("routes");
            var routes = new Routes { Id = ObjectId.NewObjectId(), Route = request.ToList() };
            var value = collection.Insert(routes);
            return StatusCode(StatusCodes.Status201Created, value.AsObjectId.ToString());
        }
        [EnableCors]
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public IActionResult GetRoutes(string id)
        {
            using var database = new LiteDatabase(local_storage_path);
            
            //Console.WriteLine(local_storage_path);
            var collection = database.GetCollection<Routes>("routes");
            var availableVehicles = collection.FindById(new ObjectId(id));
            return StatusCode(StatusCodes.Status201Created, availableVehicles.Route);
        }
    }
}