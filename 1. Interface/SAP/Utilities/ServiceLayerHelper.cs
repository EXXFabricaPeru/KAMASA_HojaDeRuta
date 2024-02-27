using B1SLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.Domain;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public class ServiceLayerHelper
    {

        private ISettingsDomain _settingsDomain;
        private string IP_ServiceLayer;
        public ServiceLayerHelper()
        {
            _settingsDomain = FormHelper.GetDomain<SettingsDomain>();
        }

        public  SLConnection Login(string Company,string UserName,string PassWord)
        {
            _settingsDomain = FormHelper.GetDomain<SettingsDomain>();
            var IP_ServiceLayer = _settingsDomain.IPServiceLayer;
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
