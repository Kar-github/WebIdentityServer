using System.Reflection.Metadata.Ecma335;
using WebApiDemo.DB.ConnectionProvider;
using WebApiDemo.Infrastructure;
using WebApiDemo.Infrastructure.DB;
using WebApiDemo.Infrastructure.Repasitories;

namespace WebApiDemo.Extensions
{
    internal static  class RepasitoryExtensions
    {
        internal static void AddRepasitories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepasitory, ProductRepasitory>(sp=>
            {
                return new ProductRepasitory(new DbContextFactory().Create());
            }
                );
        }
    }
}
