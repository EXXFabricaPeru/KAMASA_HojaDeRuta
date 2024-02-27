using System;
using System.Xml;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Code;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Resources;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Utilities;

namespace Ventura.Addon.ComisionTarjetas.Batch.Code
{
    public class ApplicationSettings
    {
        private readonly XmlDocument _applicationSettings;
        private Time? _closingReception;
        private SAPConnection _sapConnection;

        public Time ClosingReception 
            => (_closingReception ?? (_closingReception = get_setting_value("CLOSING_RECEPTION"))).Value;

        public SAPConnection Connection
            => _sapConnection ?? (_sapConnection = new SAPConnection(_applicationSettings));

        private ApplicationSettings()
        {

            try
            {
                var doc = new XmlDocument();
                doc.Load(SolutionResource.BaseConfiguration);
                _applicationSettings = doc;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(ErrorMessages.LoadApplicationSettings, exception.Message));
            }
        }

        private string get_setting_value(string propertyName)
        {
            return _applicationSettings.GetChildNode(item => item.Name == "picking_distribution")
            .GetChildNode(item => item.Name == "settings")
            .GetChildNode(item => item.Name == "setting" && item.GetAttributeValue<string>("name") == propertyName)
            .GetAttributeValue<string>("value");
        }
        
        public static ApplicationSettings MainSettings
            => new ApplicationSettings();
    }
}