using System;
using System.Text;
using Newtonsoft.Json;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public class CryptoService
    {
        public static string Encrypt<T>(T value)
        {
            var serialize = JsonConvert.SerializeObject(value);
            var bytes = Encoding.UTF8.GetBytes(serialize);
            return Convert.ToBase64String(bytes);
        }

        public static T Decrypt<T>(string value)
        {
            return default(T);
        }
    }
}