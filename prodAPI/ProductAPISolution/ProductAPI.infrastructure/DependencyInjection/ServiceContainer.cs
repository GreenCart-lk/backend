/*

using Lib.Dependencyinjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using ProductAPI.Application.Interfaces;
using ProductAPI.infrastructure.Data;
using ProductAPI.infrastructure.Repositories;

namespace ProductAPI.infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration config, IServiceCollection services)
        {
            //Add database connectivity
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);


            //create dependency injection(DI)
            services.AddScoped<IProduct, ProductRepository>();
            return service; 
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //register middileware such as:
            //global exception: handles external errors
            //listen to only API gateways
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
*/

using Lib.Dependencyinjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Application.Interfaces;
using ProductAPI.infrastructure.Data;
using ProductAPI.infrastructure.Repositories;

namespace ProductAPI.infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create dependency injection (DI)
            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register middleware such as:
            // global exception: handles external errors
            // listen to only API gateways
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
