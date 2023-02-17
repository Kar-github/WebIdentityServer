using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApiDemo.Extensions
{
    public static class AuthorizationConfigurationServiceCollectionExtensions
    {
        public static AuthenticationBuilder ConfigureAuthorization(
            this IServiceCollection services,
            string authUrl,
            string audience
            )
        {
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            return services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Authority = authUrl;
                opt.Audience = audience;
                opt.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken accessToken)
                        {
                            if (context.Principal?.Identity is ClaimsIdentity identity)
                            {
                                identity.AddClaim(new Claim("access_token", accessToken.RawData));
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
