using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace WebApiDemo.Common.CustomExtensions
{
    public static  class CommonExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            options.AddPolicy(name: "GlobalPolicy",
            policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }
                ));
        }



        //EF core Migration
        public static void MigrateDatabase<T>(this IApplicationBuilder app)
        {
           
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<T>() as DbContext;
            var pendingMigrations =  db.Database.GetPendingMigrationsAsync().Result;
            if (pendingMigrations.Any())
                db?.Database.Migrate();
        }
    }   
}
