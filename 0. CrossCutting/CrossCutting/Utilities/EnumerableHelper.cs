using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class EnumerableHelper
    {
        public delegate TK BreakableForEachAction<T, TK>(T value, ref bool doBreak);

        public delegate void ForEachParameter<T>(T item, int index, bool lastIteration);

        public static bool AsOnlyElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() == 1;
        }

        public static TSource TryFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception, TSource> actionFault)
        {
            try
            {
                return source.First(predicate);
            }
            catch (Exception exception)
            {
                return actionFault(exception);
            }
        }

        public static TSource TrySingle<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception, TSource> actionFault)
        {
            try
            {
                return source.Single(predicate);
            }
            catch (Exception exception)
            {
                return actionFault(exception);
            }
        }

        public static Type GetGenericTypeArgument(this IList enumerable)
        {
            return enumerable.GetType().GenericTypeArguments[0];
        }

        public static IEnumerable<TK> BreakableSelect<T, TK>(this IEnumerable<T> enumerable, BreakableForEachAction<T, TK> expression)
        {
            var doBreak = false;
            foreach (T item in enumerable)
            {
                yield return expression(item, ref doBreak);
                if (doBreak)
                    break;
            }
        }

        public static IList MakeGenericList(Type type)
        {
            Type listType = typeof(List<>);
            Type constructedListType = listType.MakeGenericType(type);
            return (IList) Activator.CreateInstance(constructedListType);
        }

        public static IEnumerable<T> ConcatEnumerables<T>(params IEnumerable<T>[] enumerables)
        {
            return enumerables.SelectMany(t => t);
        }

        public static T SingleOrValue<T>(this IEnumerable<T> enumerable, T value)
        {
            var @default = enumerable.SingleOrDefault();
            return EqualityComparer<T>.Default.Equals(@default, default(T)) ? value : @default;
        }

        /// <summary>Enumerates for each in this collection.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="parameter">Parameters to use in the iteration.</param>
        /// <returns>An enumerator that allows foreach to be used to process for each in this collection.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> @this, ForEachParameter<T> parameter)
        {
            T[] array = @this.ToArray();
            for (var index = 0; index < array.Length; index++)
                parameter(array[index], index, index + 1 >= array.Length);

            return array;
        }

    }
}