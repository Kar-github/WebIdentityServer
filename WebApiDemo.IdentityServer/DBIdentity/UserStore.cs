using System.Data.Entity;
using WebApiDemo.Common.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.DBIdentity
{
    public class UserStore : IUserStore
    {
        UserDbContext _db;
        public UserStore(UserDbContext db)
        {
            _db = db;
        }
        public async Task CreateUser(User user)
        {
            _db.Add(user);
             await _db.SaveChangesAsync();
        }

        public IQueryable<User> GetAllUsers(string searchText)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTypeAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> GetUserById(Guid id)
        {
            return await  _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.ToString());
            
        }

        public async  Task<IdentityUser> GetUserByUsername(string username)
        {
            return await  Task.FromResult(_db.Users.FirstOrDefault(x => x.UserName == username));
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
