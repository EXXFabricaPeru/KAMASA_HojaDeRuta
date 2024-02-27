using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class PriorityIndicatorRepository : BaseRepository
    {
        public PriorityIndicatorRepository(SAPConnector connector)
            : base(connector)
        {
        }

        public async Task<IEnumerable<PriorityIndicator>> PriorityIndicators()
        {
            const string requestUrl = @"b1s/v1/U_VS_PD_OINP";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);
            
            return response.ContentCollectionToSpecific<PriorityIndicator[]>();
        }
    }
}