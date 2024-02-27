using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace GeolocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralController : ControllerBase
    {
        private readonly SAPConnector _connector;

        public GeneralController(SAPConnector connector)
        {
            _connector = connector;
        }

        [HttpGet("central-coordinate")]
        public async Task<IActionResult> CentralCoordinate()
        {
            // IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(_connector.ServiceLayerUri, "b1s/v1/U_VS_PD_OPDS?$filter=Code eq 'PNT_LT' or Code eq 'PNT_LG'", Method.GET);
            // if (response.StatusCode != HttpStatusCode.OK)
            //     throw new Exception(response.Content);
            //
            // return Ok(response.ContentCollectionToSpecific<Coordinate[]>());

            return Ok(new List<Coordinate>
            {
                new Coordinate
                {
                    Code = "PNT_LT",
                    Description = "Punto central de latitud",
                    Type = "DE",
                    Value = new decimal(-12.0806)
                },
                new Coordinate
                {
                    Code = "PNT_LG",
                    Description = "Punto central de longitud",
                    Type = "DE",
                    Value = new decimal(-77.0867)
                }
            });
        }
    }
}