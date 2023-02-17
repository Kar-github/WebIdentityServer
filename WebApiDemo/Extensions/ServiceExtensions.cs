using WebApiDemo.Services;

namespace WebApiDemo.Extensions
{
    internal static  class ServiceExtensions
    {
        internal static void AddServices(this IServiceCollection services )
        {
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
