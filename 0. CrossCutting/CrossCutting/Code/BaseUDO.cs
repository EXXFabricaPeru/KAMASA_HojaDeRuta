using System;
using System.Reflection;
using VSVersionControl.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code
{
    [Serializable]
    public abstract class BaseUDO : BaseSAPTable
    {
        public string GetUDONameFromFieldNoRelated(string propertyName)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            return property.GetCustomAttribute<FieldNoRelated>().sAliasID;
        }
    }
}