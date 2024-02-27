using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class SettingRepository : BaseRepository
    {
        public SettingRepository(SAPConnector connector)
            : base(connector)
        {
        }

        public async Task<Setting> RetrieveStartHour()
        {
            const string requestUrl = @"b1s/v1/U_VS_PD_OPDS('DTB_HR')";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);
            
            return response.ContentCollectionToSpecific<Setting>();
        }
    }
}