using System;
using System.Xml;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class XmlExtensions
    {
        public static XmlNode GetChildNode(this XmlNode node, Func<XmlNode, bool> predicate)
        {
            XmlNodeList childNodes = node.ChildNodes;
            for (var i = 0; i < childNodes.Count; i++)
            {
                XmlNode childNode = childNodes[i];
                if (predicate.Invoke(childNode))
                    return childNode;
            }

            throw new Exception($"Don't exist child node...");
        }

        public static T GetAttributeValue<T>(this XmlNode node, string name)
        {
            if (node.Attributes == null) 
                throw new Exception($"Node don't have attribute");
            var nodeAttribute = node.Attributes[name];
            return nodeAttribute.GetValue<T>();
        }

        public static T GetValue<T>(this XmlNode node)
        {
            string nodeValue = node.Value;
            if (string.IsNullOrEmpty(nodeValue))
                throw new Exception($"Node don't have value");
            return (T) Convert.ChangeType(nodeValue, typeof(T));
        }
    }
}