using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;


using Ventura.Addon.ComisionTarjetas.ApiBeetrack.Contracts;
using Ventura.Addon.ComisionTarjetas.ApiBeetrack.Model;

namespace Ventura.Addon.ComisionTarjetas.ApiBeetrack
{
    public class ApiBeetrack
    {
        private string _url;

        public string Key { get; }

        public ApiBeetrack(string key, string url)
        {
            if (string.IsNullOrEmpty(key))
                throw new Exception("[ERROR] The API needs a key to work. Please register it");

            Key = key;
            _url = url;
        }

        public async Task<Route> MakeAsync(Route route, int idRouteSAP)
        {
            RestClient client = make_client();
            var request = make_request(Method.POST);

            var json = JsonConvert.SerializeObject(route);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception(string.Format("Status:{0}, Response: {1}", response.StatusCode, response.Content));

            var value = JsonConvert.DeserializeObject<ResponseCreateRoute>(response.Content);
            value.response.id = idRouteSAP;
            return value.response;
        }

        public Route Make(Route route)
        {
            RestClient client = make_client();
            var request = make_request(Method.POST);

            var json = JsonConvert.SerializeObject(route);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);
            if (!response.IsSuccessful)
                throw new Exception(string.Format("Status:{0}, Response: {1}", response.StatusCode, response.Content));

            var value = JsonConvert.DeserializeObject<ResponseCreateRoute>(response.Content);
            return value.response;
        }

        public Route RetrieveRoute(int id)
        {
            var client = make_client(id);
            var request = make_request(Method.GET);

            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception(string.Format("Status:{0}, Response: {1}", response.StatusCode, response.Content));

            var value = JsonConvert.DeserializeObject<ResponseGetRoute>(response.Content);
            return value.response.route;
        }

        public async Task<Route> RetrieveRouteAsync(int id)
        {
            var client = make_client(id);
            var request = make_request(Method.GET);

            IRestResponse response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new Exception(string.Format("Status:{0}, Response: {1}", response.StatusCode, response.Content));

            var value = JsonConvert.DeserializeObject<ResponseGetRoute>(response.Content);
            return value.response.route;
        }

        private RestClient make_client(int id = 0)
        {
            return new RestClient(_url + (id == default(int) ? string.Empty : $"/{id}"))
            {
                Timeout = -1
            };
        }

        private RestRequest make_request(Method method)
        {
            var request = new RestRequest(method);
            request.AddHeader("X-AUTH-TOKEN", Key);
            request.AddHeader("Content-Type", "application/json");
            return request;
        }


    }
}
