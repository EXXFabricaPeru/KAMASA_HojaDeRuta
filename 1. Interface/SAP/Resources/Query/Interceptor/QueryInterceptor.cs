using System.Reflection;
using Castle.DynamicProxy;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query.Interceptor
{
    public class QueryInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var resourceValueMethod = invocation.TargetType.BaseType.GetMethod("get_resource_tuple", BindingFlags.NonPublic | BindingFlags.Instance);
            var resourceValue = resourceValueMethod.Invoke(invocation.InvocationTarget, new object[] { invocation.Method.Name });
            invocation.ReturnValue = resourceValue;
        }
    }
}