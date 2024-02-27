using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class WarehouseRepository : BaseRepository
    {
        public WarehouseRepository(SAPConnector connector) 
            : base(connector)
        {
        }

        public async Task<Warehouse> RetrieveWarehouse(string code)
        {
            var requestUrl = $"b1s/v1/Warehouses('{code}')";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<Warehouse>();
        }

        public async Task<Warehouse> RetrieveMainWarehouse()
        {
            const string requestUrl = @"b1s/v1/Warehouses?$filter=U_VS_PD_PRNL eq 'Y'";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<Warehouse[]>().First();
        }
    }
}