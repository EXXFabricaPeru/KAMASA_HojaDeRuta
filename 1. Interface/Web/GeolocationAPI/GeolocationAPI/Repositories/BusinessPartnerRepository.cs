using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class BusinessPartnerRepository : BaseRepository
    {
        public BusinessPartnerRepository(SAPConnector connector) : base(connector)
        {
        }

        public async Task<BusinessPartner> RetrieveBusinessPartner(string cardCode)
        {
            var requestUrl = @$"b1s/v1/BusinessPartners('{cardCode}')";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<BusinessPartner>();
        }
    }
}