using System;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SAPValidValueDescription : Attribute
    {
        public string SAPField { get; }

        public SAPValidValueDescription(string sapField)
        {
            SAPField = sapField;
        }
    }
}