using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class TimeDispatchRepository : BaseRepository
    {
        public TimeDispatchRepository(SAPConnector connector) 
            : base(connector)
        {
        }

        public async Task<IEnumerable<TimeDispatch>> RetrieveTimeDispatches()
        {
            const string requestUrl = @"b1s/v1/U_VS_PD_OTPD";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<TimeDispatch[]>();
        }
    }
}