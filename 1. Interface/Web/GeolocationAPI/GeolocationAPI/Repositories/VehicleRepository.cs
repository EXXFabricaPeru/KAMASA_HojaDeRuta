using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Code;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class VehicleRepository : BaseRepository
    {
        public VehicleRepository(SAPConnector connector) 
            : base(connector)
        {
        }

        public async Task<IEnumerable<Vehicle>> RetrieveAvailableVehicle()
        {
            const string requestUrl = @"b1s/v1/U_BPP_VEHICU";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);
            
            return response.ContentCollectionToSpecific<Vehicle[]>();
        }

        public async Task<Vehicle> RetrieveVehicle(string plateId)
        {
            var requestUrl = @$"b1s/v1/U_BPP_VEHICU?$filter=U_BPP_VEPL eq '{plateId}'";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<Vehicle[]>().FirstOrRiseException(Error.ServiceLayerNotFoundException(requestUrl));
        }

        public async Task<IEnumerable<Vehicle>> RetrieveVehicleWithMaxLoads(decimal weight, decimal volume)
        {
            var requestUrl = @$"b1s/v1/U_BPP_VEHICU?$filter=U_VS_PD_CPDC le {weight:#.00} and U_VS_PD_VLDC le {volume:#.00}";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<Vehicle[]>();
        }
    }
}