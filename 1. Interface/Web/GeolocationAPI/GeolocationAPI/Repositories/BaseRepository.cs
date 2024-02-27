using GeolocationAPI.Code;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public abstract class BaseRepository
    {
        protected SAPConnector Connector { get; }
        
        protected BaseRepository(SAPConnector connector)
        {
            Connector = connector;
        }

        protected void ThrowServiceLayerException(IRestResponse response, string url)
        {
            ServiceLayerError serviceLayerError = response.ContentError();
            throw Error.ServiceLayerException(url, serviceLayerError.Message.Value);
        }

        protected void ThrowServiceLayerExceptionEdit(ServiceLayerError serviceLayerError, string url)
        {
            //ServiceLayerError serviceLayerError = response.ContentError();
            throw Error.ServiceLayerException(url, serviceLayerError.Message.Value);
        }
    }
}