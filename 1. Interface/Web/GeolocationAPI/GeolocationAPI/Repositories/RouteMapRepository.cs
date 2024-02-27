using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using GeolocationAPI.Code;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class RouteMapRepository : BaseRepository
    {
        public RouteMapRepository(SAPConnector connector)
            : base(connector)
        {
        }

        public async Task<MapRoute> RetrieveMapRoute(string clientId, string addressId)
        {
            //addressId = string.IsNullOrEmpty(addressId) ? addressId : addressId.Replace("'", "%22");
            //addressId = HttpUtility.UrlEncode(addressId);

            addressId = string.IsNullOrEmpty(addressId) ? addressId : addressId.Replace("'", "''");
            addressId = string.IsNullOrEmpty(addressId) ? addressId : addressId.Replace("&", "%26");
            addressId = string.IsNullOrEmpty(addressId) ? addressId : addressId.Replace("#", "%23");
            var requestUrl = @$"b1s/v1/VS_PD_OMPR?$filter=U_VS_PD_COCL eq '{clientId}' and U_VS_PD_COEN eq '{addressId}'";


            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<MapRoute[]>().FirstOrDefault();
        }

        public async Task<int> InsertMapRoute(MapRoute mapRoute)
        {
            const string requestUrl = @"b1s/v1/VS_PD_OMPR";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.POST, mapRoute);
            JObject jObject = JObject.Parse(response.Content);
            JToken jToken = jObject["DocEntry"];
            var value = jToken?.Value<int>();
            if (value == null)
                throw Error.ServiceLayerNotInsertRecordException(requestUrl);

            return value.Value;
        }

        public async Task UpdateMapRoute(MapRoute mapRoute)
        {
            var requestUrl = @$"b1s/v1/VS_PD_OMPR({mapRoute.DocEntry})";
            await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.PATCH, mapRoute);
            await RetrieveMapRoute(mapRoute.ClientId, mapRoute.AddressId);
        }
    }
}