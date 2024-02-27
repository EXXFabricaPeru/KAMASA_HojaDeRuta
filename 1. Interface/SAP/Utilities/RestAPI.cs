using System;
using System.Net;
using DisposableSAPBO;
using Newtonsoft.Json;
using RestSharp;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Resources;
using System.IO;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public class RestAPI
    {
        public static object PostRequest(string urlBase, string urlMethod, object body)
        {
            var restResponse = basic_post_request(urlBase, urlMethod, body);
            return JsonConvert.DeserializeObject(restResponse.Content);
        }

        public static T PostRequest<T>(string urlBase, string urlMethod, object body)
        {
            var restResponse = basic_post_request(urlBase, urlMethod, body);
            return JsonConvert.DeserializeObject<T>(restResponse.Content);
        }

        private static IRestResponse basic_post_request(string urlBase, string urlMethod, object body)
        {
            var restClient = new RestClient(new Uri(urlBase));
            restClient.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            IRestRequest restRequest = new RestRequest(urlMethod, Method.POST)
                .AddJsonBody(body, "application/json");
            string jsonString = JsonConvert.SerializeObject(body);
            restClient.Timeout = (300000);
            var restResponse = restClient.Execute(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception(string.Format(ErrorMessages.NotConnectWithAPI, urlBase, restResponse.Content));

            return restResponse;
        }



        public static async Task<T> PostRequestAsync<T>(string urlBase, string urlMethod, object body)
        {
            var restResponse = await basic_post_request_async(urlBase, urlMethod, body);
            return JsonConvert.DeserializeObject<T>(restResponse.Content);
        }
        private static async Task<IRestResponse> basic_post_request_async(string urlBase, string urlMethod, object body)
        {
            
            var restClient = new RestClient(new Uri(urlBase));
            restClient.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            IRestRequest restRequest = new RestRequest(urlMethod, Method.POST)
                .AddJsonBody(body, "application/json");
            string jsonString = JsonConvert.SerializeObject(body);
            restClient.Timeout = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
            var restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception(string.Format(ErrorMessages.NotConnectWithAPI, urlBase, restResponse.Content));

            return restResponse;
        }

    }
}