using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class TimeWindowDispatchRepository : BaseRepository
    {
        public TimeWindowDispatchRepository(SAPConnector connector)
            : base(connector)
        {
        }

        public async Task<TimeWindowDispatch> RetrieveTimeWindowDispatch(string id)
        {
            var requestUrl = @$"b1s/v1/VS_PD_OVHR('{id}')";
            //var requestUrl = @$"b1s/v1/VS_VH'{id}')";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<TimeWindowDispatch>();
        }
    }
}