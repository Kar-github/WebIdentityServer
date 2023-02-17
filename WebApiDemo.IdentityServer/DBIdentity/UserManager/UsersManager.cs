using WebApiDemo.Common.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.DBIdentity
{
    public class UsersManager : IUsersManager
    {
        IUserStore _store;
        public UsersManager(IUserStore store)
        {
            _store = store;
        }
        public  Task CreateUser(User user)
        {
           return  _store.CreateUser(user);
        }


        public Task<User> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> GetUserByUserName(string username)
        {
            return _store.GetUserByUsername(username);
        }
    }
}
