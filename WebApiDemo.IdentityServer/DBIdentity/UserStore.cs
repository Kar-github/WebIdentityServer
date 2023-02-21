using System.Data.Entity;
using WebApiDemo.Common.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Application.Models;

namespace IdentityServer.DBIdentity
{
    public class UserStore : IUserStore
    {
        UserDbContext _db;
        public UserStore(UserDbContext db)
        {
            _db = db;
        }
        public async Task CreateUser(ApplicationUser user)
        {
            _db.Add(user);
            await SaveChangesAsync();
        }

        public Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            return Task.FromResult(_db.Users.AsNoTracking());
        }


        public async Task<ApplicationUser> GetUserById(Guid id)
        {
            return await  _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.ToString());
            
        }

        public async  Task<ApplicationUser> GetUserByUsername(string username)
        {
            return await  Task.FromResult(_db.Users.FirstOrDefault(x => x.UserName == username));
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public Task<bool> Update(ApplicationUser user)
        {
            try
            {
                _db.Users.Update(user);
                _db.SaveChanges();
                return Task.FromResult(true);
            }
            catch { return Task.FromResult(false); }
            
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
             
            try
            {
                _db.Users.Update(user);
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
