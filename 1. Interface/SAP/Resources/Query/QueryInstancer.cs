using Autofac;
using Autofac.Extras.DynamicProxy;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query.Interceptor;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query
{
    public static class QueryInstancer
    {
        public static T Make<T>(QueryType queryType) where T : SAPQueryManager
        {
            var builder = new ContainerBuilder();
            builder.RegisterType(typeof(T))
                .EnableClassInterceptors()
                .InterceptedBy(typeof(QueryInterceptor));

            builder.Register(interceptor => new QueryInterceptor());
            IContainer container = builder.Build();
            return container.Resolve<T>(new NamedParameter("queryType", queryType));
        }
    }
}