using System;
using SAPbobsCOM;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SAPObjectAttribute : Attribute
    {
        public BoObjectTypes SapTypes { get; }

        public SAPObjectAttribute(BoObjectTypes sapTypes)
        {
            SapTypes = sapTypes;
        }
    }
}