using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Models;
using GeolocationAPI.SAPComponents;
using GeolocationAPI.Utilities;
using RestSharp;

namespace GeolocationAPI.Repositories
{
    public class TransporterRepository : BaseRepository
    {
        public TransporterRepository(SAPConnector connector) 
            : base(connector)
        {
        }

        public async Task<IEnumerable<Transporter>> RetrieveAll()
        {
            const string requestUrl = @"b1s/v1/VS_PD_OTRD";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);
            
            return response.ContentCollectionToSpecific<Transporter[]>();
        }
        public async Task<Transporter_Vehicle> RetrieveTransporterVehicle(string code)
        {
            var requestUrl = @$"b1s/v1/$crossjoin(VS_PD_OTRD,VS_PD_OTRD/VS_PD_TRD2Collection)?$expand=VS_PD_OTRD($select=Code,U_VS_PD_PRDS as Priority),VS_PD_OTRD/VS_PD_TRD2Collection($select=U_VS_PD_CDVH as Code,U_VS_PD_CDVH as Name,U_VS_PD_CDVH,U_VS_PD_CPDC as WeightCapacity ,U_VS_PD_VLDC as VolumeCapacity,U_VS_PD_SRVH as HasRefrigeration,U_VS_PD_ACVH as IsAvailable )&$filter=VS_PD_OTRD/Code eq VS_PD_OTRD/VS_PD_TRD2Collection/Code and VS_PD_OTRD/VS_PD_TRD2Collection/U_VS_PD_CDVH  eq '{code}'";
            IRestResponse response = await RestClientHelper.ExecuteServiceLayerRequest(Connector.ServiceLayerUri, requestUrl, Method.GET);
            if (response.StatusCode != HttpStatusCode.OK)
                ThrowServiceLayerException(response, requestUrl);
            
            return response.ContentCollectionToSpecific<Transporter_Vehicle>();
        }
    
    }
}