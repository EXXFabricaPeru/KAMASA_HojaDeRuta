﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class DeliveryControlRepository : BaseRepository
    {
        public DeliveryControlRepository(SAPConnector connector) 
            : base(connector)
        {
        }

        public async Task<IEnumerable<DeliveryControl>> RetrieveDeliveryControls()
        {
            const string requestUrl = @"b1s/v1/VS_PD_OREN";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);
            
            return response.ContentCollectionToSpecific<DeliveryControl[]>();
        }
    }
}