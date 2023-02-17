using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using WebApiDemo.DB.ConnectionProvider;
using WebApiDemo.Infrastructure;
using WebApiDemo.Services.Infrastructure;

namespace WebApiDemo.Extensions
{
    internal static class CustomExtensions
    {
        internal static void ConfigConnection(this IServiceCollection services)
        {
            services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
        }
        public static void ConfigureMediatorContainer(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatorModule(typeof(Program).GetTypeInfo().Assembly)));

        }
        //internal static void MigrateDatabase(IApplicationBuilder builder)
        //{
        //    using var scope = builder.ApplicationServices.CreateScope();

        //    var services = scope.ServiceProvider;

        //    var context = services.GetService<WebApiDemoContext>();
        //    context?.Database.Migrate();
        //}
    }
}
