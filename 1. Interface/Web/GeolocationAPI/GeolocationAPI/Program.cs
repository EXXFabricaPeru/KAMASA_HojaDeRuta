using GeolocationAPI.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GeolocationAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            make_host_builder(args)
                .Build()
                .Run();
        }

        private static IHostBuilder make_host_builder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(configure_geolocalization_web_host);

        private static void configure_geolocalization_web_host(IWebHostBuilder hostBuilder)
        {
            hostBuilder.UseStartup<Startup>()
                .ConfigureAppConfiguration(geolocalization_configuration)
                .UseIIS()
                .UseIISIntegration();

            void geolocalization_configuration(WebHostBuilderContext hostingContext, IConfigurationBuilder configurationBuilder)
            {
                IWebHostEnvironment environment = hostingContext.HostingEnvironment;

                configurationBuilder.AddJsonFile($"{FileNames.APPLICATION_SETTINGS}.json", true, true)
                    .AddJsonFile($"{FileNames.APPLICATION_SETTINGS}.{environment.EnvironmentName}.json", true, true);
            }
        }
    }
}