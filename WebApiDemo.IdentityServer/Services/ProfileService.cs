using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var sub = context.Subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == sub);
            context.IssuedClaims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(JwtClaimTypes.Subject, user.Id),
                new System.Security.Claims.Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new System.Security.Claims.Claim(JwtClaimTypes.Email, user.Email ?? "-"),
                new System.Security.Claims.Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber ?? "-"),
            };
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var sub = context.Subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                context.IsActive = false;
                return;
            }
            context.IsActive = true;
        }
    }
}