using System;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SAPColumnAttribute : Attribute
    {
        public string Name { get; }

        public bool SystemField { get; }

        public bool DetailLine { get; set; }

        public SAPColumnAttribute(string name = "", bool systemField = true, bool detailLine = false)
        {
            Name = name;
            SystemField = systemField;
            DetailLine = detailLine;
        }
    }
}