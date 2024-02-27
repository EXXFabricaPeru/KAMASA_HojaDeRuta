using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class VelocityRepository : BaseRepository
    {
        public VelocityRepository(SAPConnector connector) 
            : base(connector)
        {
        }

        public async Task<IEnumerable<Velocity>> RetrieveVelocities()
        {
            const string requestUrl = @"b1s/v1/U_VS_PD_OCVL";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);
            
            return response.ContentCollectionToSpecific<Velocity[]>();
        }
    }
}