using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Common.Models;

namespace IdentityServer.DBIdentity
{
    public interface IUsersManager
    {
        public Task CreateUser(User user);
        public Task<User> GetUserById(Guid id);
        public Task<IdentityUser> GetUserByUserName(string username);
    }
}
