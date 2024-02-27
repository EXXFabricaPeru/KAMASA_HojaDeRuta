using B1SLayer;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Code
{
    public class ServiceLayerHelper
    {
        //private ISettingsDomain _settingsDomain;
        private string IP_ServiceLayer;
        public ServiceLayerHelper()
        {
            
        }

        public SLConnection Login(Company _company,string Company, string UserName, string PassWord)
        {
            
            BaseOPDSRepository _settingsDomain = new UnitOfWork(_company).SettingsRepository;

            var IP_ServiceLayer = _settingsDomain.Setting(OPDS.Codes.IP_SERVICE_LAYER);

            var serviceLayer = new SLConnection(IP_ServiceLayer.Value, Company, UserName, PassWord);
            serviceLayer.AfterCall(async call =>
            {
                Console.WriteLine($"Request: {call.HttpRequestMessage.Method} {call.HttpRequestMessage.RequestUri}");
                Console.WriteLine($"Body sent: {call.RequestBody}");
                Console.WriteLine($"Response: {call.HttpResponseMessage?.StatusCode}");
                Console.WriteLine(await call.HttpResponseMessage?.Content?.ReadAsStringAsync());
                Console.WriteLine($"Call duration: {call.Duration.Value.TotalSeconds} seconds");
            });

            return serviceLayer;
        }
    }
}
