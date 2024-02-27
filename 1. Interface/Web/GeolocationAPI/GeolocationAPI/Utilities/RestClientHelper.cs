using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommonUtilities.Enumerated;
using CommonUtilities.Exceptions;
using CommonUtilities.Extensions;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace GeolocationAPI.Utilities
{
    public static class RestClientHelper
    {
        public static async Task<IRestResponse> ExecuteDirectionsMapboxRequest(string key, (double latitude, double longitude) startPoint,
            (double latitude, double longitude) endPoint, TransferOrder transferOrder)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                //HttpClientHandler clientHandler = new HttpClientHandler();
                //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var urlMethod =
                       $"{startPoint.longitude:#.00000},{startPoint.latitude:#.00000};{endPoint.longitude:#.00000},{endPoint.latitude:#.00000}?" +
                       $"overview=full&access_token={key}";
                var restClient = new RestClient("https://api.mapbox.com/directions/v5/mapbox/driving/"+ urlMethod);
                restClient.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
                clientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls | System.Security.Authentication.SslProtocols.Tls11;


                HttpClient client = new HttpClient(clientHandler);



                IRestRequest restRequest = new RestRequest(Method.GET)
                    .SetContentType(ContentType.ApplicationJson);

                //var response = client.GetAsync("https://api.mapbox.com/directions/v5/mapbox/driving/" + urlMethod).Result;
                return await restClient.ExecuteAsync(restRequest);
            }
            catch (Exception ex)
            {

                throw new Exception("VAL Error en los puntos de laitud o longitud revise las direcciones de la OT "+ transferOrder.DocNum);
            }
          
        }

        public static async Task<IRestResponse> ExecuteServiceLayerRequest(Uri baseUrl, string requestUrl, Method method, object body = null)
        {
            var restClient = new RestClient(baseUrl);
            restClient.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            IRestRequest restRequest = new RestRequest(requestUrl, method)
                .SetContentType(ContentType.ApplicationJson)
                .AddCookie(SAPConnector.SESSION_NAME, SAPConnector.SESSION)
                .AddCookie(SAPConnector.ROUTE_NAME, SAPConnector.ROUTE)
                .AddHeader("Prefer", "odata.maxpagesize=200");

            if ((method == Method.PATCH || method == Method.POST) && body != null)
            {
                restRequest.AddJsonBody(body, "application/json");
                restClient.UseNewtonsoftJson(new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                });
            }

            return await restClient.ExecuteAsync(restRequest);
        }

        public static T ContentCollectionToSpecific<T>(this IRestResponse response)
        {
            JObject responseContent = JObject.Parse(response.Content);
            if (!typeof(T).IsArray)
                return responseContent.ToObject<T>();

            JToken values = responseContent["value"];
            if (values == null)
                throw new HTTPException(HttpStatusCode.InternalServerError.ToInt32(), "[service layer error] SAP don't return correct structure");

            return values.ToObject<T>();
        }

        public static ServiceLayerError ContentError(this IRestResponse response)
        {
            JObject responseContent = JObject.Parse(response.Content);
            JToken error = responseContent["error"];
            if (error == null)
                throw new HTTPException(HttpStatusCode.InternalServerError.ToInt32(), "[service layer error] SAP don't return correct structure");
            return error.ToObject<ServiceLayerError>();
        }
    }
}