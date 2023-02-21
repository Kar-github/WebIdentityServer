using WebApiDemo.Common.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Application.Models;

namespace IdentityServer.DBIdentity
{
    public class UsersManager : IUsersManager
    {
        IUserStore _store;
        public UsersManager(IUserStore store)
        {
            _store = store;
        }
        public  Task CreateUser(ApplicationUser user)
        {
           return  _store.CreateUser(user);
        }

        public Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            return _store.GetAllUsers();
        }

        public Task<ApplicationUser> GetUserById(Guid id)
        {
            return _store.GetUserById(id);
        }

        public Task<ApplicationUser> GetUserByUserName(string username)
        {
            return _store.GetUserByUsername(username);
        }

        public Task<bool> UpdateUser(ApplicationUser user)
        {
            return _store.Update(user);
        }

        public async  Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            return await _store.UpdateAsync(user);
        }
    }
}
