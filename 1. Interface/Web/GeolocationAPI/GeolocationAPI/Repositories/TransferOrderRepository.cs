using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class TransferOrderRepository : BaseRepository
    {
        public TransferOrderRepository(SAPConnector connector)
            : base(connector)
        {
        }

        public async Task<IEnumerable<TransferOrder>> OpenTransferOrders()
        {
            const string requestUrl = @"b1s/v1/VS_PD_ORTR?$filter=U_VS_PD_STAD eq 'O' and U_VS_PD_VARE eq 'N' and Canceled eq 'N' ";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<TransferOrder[]>();
        }

        public async Task<TransferOrder> RetrieveTransferOrder(int documentEntry)
        {
            var requestUrl = @$"b1s/v1/VS_PD_ORTR({documentEntry})";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<TransferOrder>();
        }
    }
}