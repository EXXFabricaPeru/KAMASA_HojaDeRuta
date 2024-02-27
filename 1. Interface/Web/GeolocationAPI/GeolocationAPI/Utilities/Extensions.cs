using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GeolocationAPI.Utilities
{
    public static class Extensions
    {
        public static T IfDefault<T>(this T value, Func<T, T> function) where T : struct
        {
            return Equals(value, default(T)) ? function(value) : value;
        }

        public static IEnumerable<TK> DistinctElements<T, TK>(this IEnumerable<T> enumerable, Expression<Func<T, TK>> expression)
        {
            IList<TK> viewValues = new List<TK>();

            foreach (var item in enumerable)
            {
                var value = expression.Compile().Invoke(item);
                if (viewValues.Contains(value))
                    continue;
                viewValues.Add(value);
            }

            return viewValues;
        }

        public static T FirstOrRiseException<T>(this IEnumerable<T> enumerable, Exception exception, Func<T, bool> predicate = null)
        {
            T element = predicate == null ? enumerable.FirstOrDefault() : enumerable.FirstOrDefault(predicate);
            if (object.Equals(element, default(T)))
                throw exception;
            return element;
        }
    }
}