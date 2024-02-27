using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Code;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class AssignedCertifyRepository : BaseRepository
    {
        public AssignedCertifyRepository(SAPConnector connector)
            : base(connector)
        {
        }

        public async Task<AssignedCertify> RetrieveAssignationOfBusinessPartner(string cardCode)
        {
            ServiceLayerError serviceLayerError = new ServiceLayerError();
            serviceLayerError.Message = new ServiceLayerErrorMessage();
            var requestUrl = @$"b1s/v1/VS_PD_OACD?$filter=U_VS_PD_CDET eq '{cardCode}' and U_VS_PD_TPET eq 'CL'";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            //try
            //{
            var x = response.ContentCollectionToSpecific<AssignedCertify[]>();
                if (x.Length > 0)
                    return x[0];
                else
                {
                 
                    //serviceLayerError.Message.Value = Error.CertificatesNotAssigned(cardCode).Message;
                    //ThrowServiceLayerExceptionEdit(serviceLayerError, requestUrl);
                    throw new System.Net.Http.HttpRequestException(Error.CertificatesNotAssigned(cardCode).Message + " URL: "+ requestUrl );
                }
                   
            //}
            //catch (Exception ex)
            //{
               
            //    serviceLayerError.Message.Value = Error.CertificatesNotAssigned(cardCode).Message;
            //    ThrowServiceLayerExceptionEdit(serviceLayerError, requestUrl);
            //    throw new Exception(requestUrl + " - " + Error.CertificatesNotAssigned(cardCode).Message);
            //    //ThrowServiceLayerException(response, requestUrl);
            //}
            //return response.ContentCollectionToSpecific<AssignedCertify[]>().FirstOrRiseException(Error.CertificatesNotAssigned(cardCode));
        }

        public async Task<IEnumerable<AssignedCertify>> RetrieveAllAssignationOfDriver()
        {
            const string requestUrl = @"b1s/v1/VS_PD_OACD?$filter=U_VS_PD_TPET eq 'CD'";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<AssignedCertify[]>();
        }

        public async Task<IEnumerable<AssignedCertify>> RetrieveAllAssignationOfAuxiliary()
        {
            const string requestUrl = @"b1s/v1/VS_PD_OACD?$filter=U_VS_PD_TPET eq 'AX'";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<AssignedCertify[]>();
        }

        public async Task<IEnumerable<AssignedCertify>> RetrieveAllAssignationOfAuxiliaryByCode(string code)
        {
            var requestUrl = @$"b1s/v1/VS_PD_OACD?$filter=U_VS_PD_CDET eq '{code}'";
            //var requestUrl = @$"b1s/v1/VS_PD_OACD?$filter=Code eq '{code}'";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);


            AssignedCertify[] assignedCertifies = response.ContentCollectionToSpecific<AssignedCertify[]>();
            if (assignedCertifies.Length <= 0)
                return assignedCertifies;

            requestUrl = @$"b1s/v1/VS_PD_OACD?$filter=U_VS_PD_NMET eq '{assignedCertifies[0].ReferenceEntityName}' and U_VS_PD_TPET eq 'AX'";
            response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);

            return response.ContentCollectionToSpecific<AssignedCertify[]>();
        }
    }
}