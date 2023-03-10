using IdentityModel;
using IdentityServer.Application.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionstring)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<UserDbContext>(options=>options.UseSqlServer(connectionstring));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();
            services.AddOperationalDbContext(options=>
            {
                options.ConfigureDbContext = db =>
                db.UseSqlServer(connectionstring, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));

            });
            services.AddConfigurationDbContext(options=>
            {
                options.ConfigureDbContext = db =>
               db.UseSqlServer(connectionstring, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<UserDbContext>();
            ctx.Database.Migrate();
            EnsureUsers(scope);
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user1 = usermanager.FindByNameAsync("user1").Result;
            if(user1 is null)
            {
                user1 = new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user111115225656512@mail.ru",
                    EmailConfirmed = true,
                };
                var result = usermanager.CreateAsync(user1, "Kar2522!").Result;
                if(!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = usermanager.AddClaimsAsync(
                    user1,
                    new System.Security.Claims.Claim[]
                    {
                        new System.Security.Claims.Claim(JwtClaimTypes.Name,"user1"),
                        new System.Security.Claims.Claim(JwtClaimTypes.GivenName,"user1")
                    }
                    ).Result;
                if(!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            
            
            if(!context.Clients.Any())
            {
                foreach(var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            if(!context.IdentityResources.Any())
            {
                foreach(var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            if(!context.ApiResources.Any())
            {
                foreach(var resource in Config.ApiResources)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
