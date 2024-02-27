using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class GenericHelper
    {
        public static string GetUdoId<T>() where T : BaseUDO
        {
            var udoType = typeof(T);
            var udoAttribute = udoType.GetCustomAttribute<Udo>();
            if (udoAttribute == null)
                throw new Exception($"[ERROR] Bad set UDO attributes into class'{udoType.Name}'");

            return udoAttribute.UdoName;
        }

        public static T MakeInstanceWithPrivateConstructors<T>(BindingFlags bindingFlags, Type[] parameterModifiers, object[] parameters)
        {
            Type classType = typeof(T);
            object instance = classType
                .GetConstructor(bindingFlags, null, parameterModifiers, null)?.Invoke(parameters);

            if (instance == null)
                throw new Exception($"[Error] The class '{classType.Name}' is not implemented correctly");

            return instance.To<T>();
        }

        public static object MakeInstance(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }

        public static T MakeInstance<T>(params object[] parameters)
        {
            return (T) MakeInstance(typeof(T), parameters);
        }

        /// <summary>
        /// Only available for UDF with 'ValidValues' property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="valueKey"></param>
        /// <returns></returns>
        public static string GetDescriptionFromValue(PropertyInfo property, string valueKey)
        {
            return property.GetCustomAttributes<Val>()
                .Single(item => item.Key == valueKey)
                .Value;
        }

        /// <summary>
        /// Only available for UDF with 'ValidValues' property
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="valueKey"></param>
        /// <returns></returns>
        public static string GetDescriptionFromValue<T, TK>(Expression<Func<T, TK>> mapping, string valueKey)
        {
            var property = GetPropertyForExpression(mapping);
            return GetDescriptionFromValue(property, valueKey);
        }

        public static PropertyInfo GetPropertyForExpression<T, TK>(Expression<Func<T, TK>> mapping)
        {
            var memberExpression = mapping.Body as MemberExpression;
            var propertyInfo = memberExpression?.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new Exception();
            return propertyInfo;
        }

        public static void ReleaseCOMObjects(params object[] objects)
        {
            for (var i = default(int); i < objects.Length; i++)
            {
                object comObject = objects[i];
                if (comObject == null)
                    continue;
                Marshal.ReleaseComObject(comObject);
                comObject = null;
            }

            GC.Collect();
        }

        public static bool IsDefaultValue(Type valueType, object value)
        {
            object defaultValue = null;
            if (valueType.IsValueType)
                defaultValue = Activator.CreateInstance(valueType);
            return Equals(value, defaultValue);
        }
    }
}