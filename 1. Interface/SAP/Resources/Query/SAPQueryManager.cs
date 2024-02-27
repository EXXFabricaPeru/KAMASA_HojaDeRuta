using System.Reflection;
using System.Resources;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query
{
    public abstract class SAPQueryManager
    {
        private readonly ResourceManager _resourceManager;

        public string Category { get; }

        protected SAPQueryManager(string category, QueryType type)
        {
            Category = category;
            _resourceManager = new ResourceManager(get_menu_namespace(type), Assembly.GetAssembly(typeof(SAPQueryManager)));
        }

        private string get_menu_namespace(QueryType type)
        {
            var resourceNamespace = this.GetCustomAttribute<ResourceNamespace>();
            return $"{resourceNamespace.NameSpace}.Query.{(type == QueryType.SQL ? "SQL" : "HANA")}_querys";
        }

        private ElementTuple<string> get_resource_tuple(string methodName)
        {
            string currentPropertyName = methodName.Substring(4);
            string resourceValue = _resourceManager.GetString(currentPropertyName);
            return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        }
    }
}