using System;
using GeolocationAPI.Filters;
using GeolocationAPI.Repositories;
using GeolocationAPI.SAPComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeolocationAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            services.AddControllers(options =>
            {
                options.Filters.Add<SAPConnectionFilter>();
            });
            var tokenSection = Configuration.GetSection("MapboxToken");
            IConfigurationSection section = Configuration.GetSection(SAPConnector.SECTION_NAME);
            var sapConnector = section.Get<SAPConnector>();
            var unitOfWork = new UnitOfWork(sapConnector);
            services.AddSingleton(sapConnector);
            services.AddSingleton(unitOfWork);
            services.AddSingleton(tokenSection.Value);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
