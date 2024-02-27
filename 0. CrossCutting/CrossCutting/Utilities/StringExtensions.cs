using System;
using System.Linq;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class StringExtensions
    {
        public static string IfNotNullOrEmpty(this string @string, Func<string, string> function)
        {
            return !string.IsNullOrEmpty(@string) ? function(@string) : string.Empty;
        }

        public static string IfIsNullOrEmpty(this string @string, string returnValue)
        {
            return string.IsNullOrEmpty(@string) ? returnValue : @string;
        }

        public static string SubstringStartAt(this string @string, int numerator)
        {
            return @string.Substring(numerator, @string.Length - numerator);
        }

        public static int ToIntegerFormatHour(this string @string)
        {
            if (string.IsNullOrEmpty(@string))
            {
                return default(int);
            }

            var strings = @string.Split(':');
            var value = strings.Aggregate(string.Empty, (current, t) => current + t);
            return Convert.ToInt32(value);
        }

        public static bool IsAnyValueEquals(this string @string, params string[] values)
        {
            return values.Any(value => @string == value);
        }

        public static bool IsAllNotEquals(this string @string, params string[] values)
        {
            return values.All(value => @string != value);
        }
    }
}