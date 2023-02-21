using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityServer.DBIdentity;
using IdentityServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using IdentityServer4;
using static Org.BouncyCastle.Math.EC.ECCurve;
using IdentityServer.Application.Models;

namespace IdentityServer.Extensions
{
    public static  class ServiceExtenssions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IUsersManager, UsersManager>();
        }
        public static void ConfigIdentityServer(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityServer().ConfigIdentityServer(config);
               
                
        }
        private static void ConfigIdentityServer(this IIdentityServerBuilder server, IConfiguration config)
        {
            server.AddAspNetIdentity<ApplicationUser>();
            server.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                b.UseSqlServer(config.GetConnectionString("DefaultConnection"), opt => opt.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
            });
            server.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                b.UseSqlServer(config.GetConnectionString("DefaultConnection"), opt => opt.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
            });
            try
            {
                server.AddSigningCredential(LoadSigningCertificate(config).Result);
            }
            catch { server.AddDeveloperSigningCredential(); }
            



            //.AddDeveloperSigningCredential()
            //.AddInMemoryClients(Config.Clients)
            //.AddInMemoryApiResources(Config.ApiResources)
            //.AddInMemoryApiScopes(Config.ApiScopes)
            //.AddInMemoryIdentityResources(Config.IdentityResources)
            //.AddTestUsers(Config.TestUsers)

        }
        private async static  Task<X509Certificate2> LoadSigningCertificate(IConfiguration config)
        {
            var keyVaultUrl = config.GetSection("AzureKeyVaultSettings")["KeyVaultUri"];
            var secretName = config.GetSection("AzureKeyVaultSettings")["CertificateName"];
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var _client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var secret =await  _client.GetSecretAsync(keyVaultUrl, secretName);
            var privateKeyBytes = Convert.FromBase64String(secret.Value);
            var certificate= new X509Certificate2(privateKeyBytes, string.Empty, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return certificate;
        }
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 6;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                config.Lockout.MaxFailedAccessAttempts = 20;

            }).AddEntityFrameworkStores<UserDbContext>()
              .AddDefaultTokenProviders();

        }
        public static void AddGoogleAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication().
                AddGoogle((options)=>
                {
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.ClientId = config.GetSection("GoogleAuthenticationSettings")["GoogleClientId"];
                    options.ClientSecret = config.GetSection("GoogleAuthenticationSettings")["GoogleClientSecret"];
                    options.AuthorizationEndpoint += "?prompt=select_account";
                    //options.CallbackPath = "/signin-google";
                });
        }
        public static void AddJwt(this IServiceCollection services,IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey=true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });
            
               

        }
    }
}
