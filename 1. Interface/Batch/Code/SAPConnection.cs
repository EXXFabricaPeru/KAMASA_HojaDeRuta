using System.Xml;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Utilities;

namespace Ventura.Addon.ComisionTarjetas.Batch.Code
{
    public class SAPConnection
    {
        private readonly XmlNode _node;

        public SAPConnection(XmlNode node)
        {
            _node = node.GetChildNode(item => item.Name == "picking_distribution")
                .GetChildNode(item => item.Name == "sap_connection");
        }

        public string Server 
            => _node.GetChildNode(item => item.Name == "server").GetAttributeValue<string>("value");

        public string Type => _node.GetChildNode(item => item.Name == "type").GetAttributeValue<string>("value");

        public string Company => _node.GetChildNode(item => item.Name == "company").GetAttributeValue<string>("value");

        public string UserName => _node.GetChildNode(item => item.Name == "username").GetAttributeValue<string>("value");

        public string Password => _node.GetChildNode(item => item.Name == "password").GetAttributeValue<string>("value");
    }
}