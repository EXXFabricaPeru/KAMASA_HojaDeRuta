using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectorController : ControllerBase
    {
        private readonly SAPConnector _connector;

        public SectorController(SAPConnector connector)
        {
            _connector = connector;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(_connector.ServiceLayerUri, "b1s/v1/VS_PD_OPDG", Method.GET);
            // if (response.StatusCode != HttpStatusCode.OK)
            //     throw new Exception(response.Content);
            //
            // return Ok(response.ContentCollectionToSpecific<Sector[]>());
            return Ok();
        }
    }
}
