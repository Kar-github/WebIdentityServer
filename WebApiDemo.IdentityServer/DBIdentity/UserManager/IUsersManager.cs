using IdentityServer.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Common.Models;

namespace IdentityServer.DBIdentity
{
    public interface IUsersManager
    {
        public Task CreateUser(ApplicationUser user);
        public Task<ApplicationUser> GetUserById(Guid id);
        public Task<ApplicationUser> GetUserByUserName(string username);
        public Task<bool> UpdateUser(ApplicationUser user);
        public Task<bool> UpdateUserAsync(ApplicationUser user);
        public Task<IQueryable<ApplicationUser>> GetAllUsers();
    }
}
