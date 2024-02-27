using System;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UDOFatherReferenceAttribute : Attribute
    {
        public string UDOId { get; }

        public int ChildNumber { get; }

        public UDOFatherReferenceAttribute(string id, int childNumber)
        {
            UDOId = id;
            ChildNumber = childNumber;
        }
    }
}