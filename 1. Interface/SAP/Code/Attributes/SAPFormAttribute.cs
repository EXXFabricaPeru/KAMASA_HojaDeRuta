using System;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SAPFormAttribute : Attribute
    {
        public SAPFormAttribute(string id, string description)
        {
            Id = id;
            Description = description;
        }

        public string Id { get; }
        public string Description { get; }
    }
}