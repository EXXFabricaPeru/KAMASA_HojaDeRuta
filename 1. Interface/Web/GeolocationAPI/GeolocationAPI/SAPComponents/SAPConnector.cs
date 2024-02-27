// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Collections.Generic;
using System.Net;
using CommonUtilities.Enumerated;
using CommonUtilities.Exceptions;
using CommonUtilities.Extensions;
using RestSharp;

namespace GeolocationAPI.SAPComponents
{
    public class SAPConnector
    {
        private static readonly Uri LOGIN_RELATIVE_PATH = new Uri("b1s/v1/Login", UriKind.Relative);

        public const string SESSION_NAME = "B1SESSION";
        public const string ROUTE_NAME = "ROUTEID";
        public const string SECTION_NAME = "SAPConnector";
        
        public static string SESSION { get; private set; }
        public static string ROUTE { get; private set; }
        
        public string Ip { get; set; }

        public string Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Company { get; set; }

        public Uri ServiceLayerUri
            => new Uri($"https://{Ip}:{Port}");

        public object SAPCredential
            => new
            {
                UserName = User,
                Password,
                CompanyDB = Company
            };

        public void Connect()
        {
            var restClient = new RestClient(ServiceLayerUri);
            restClient.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            var restRequest = new RestRequest(LOGIN_RELATIVE_PATH, Method.POST);
            restRequest.SetContentType(ContentType.ApplicationJson)
                .AddJsonBody(SAPCredential);

            IRestResponse response = restClient.Execute(restRequest);
            HttpStatusCode responseCode = response.StatusCode;
            if (responseCode != HttpStatusCode.OK)
                throw new HTTPException(responseCode.ToInt32(), response.Content + "Error de conexión con el SL");
            set_session(response.Cookies);
        }

        private void set_session(IList<RestResponseCookie> cookies)
        {
            var httpException = new HTTPException(HttpStatusCode.InternalServerError.ToInt32(), "[service layer error] SAP don't return session cookie");
            SESSION = cookies.SingleOrRiseException(httpException, cookie => cookie.Name == SESSION_NAME).Value;
            ROUTE = cookies.SingleOrRiseException(httpException,cookie => cookie.Name == ROUTE_NAME).Value;
        }
    }
}