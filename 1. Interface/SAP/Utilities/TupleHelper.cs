using System;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class TupleHelper
    {
        public static void IfFailureThrowException(this Tuple<bool, string> response)
        {
            if (!response.Item1)
                throw new Exception(response.Item2);
        }
    }
}