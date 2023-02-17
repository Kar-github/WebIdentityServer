using Microsoft.AspNetCore.Identity;
using WebApiDemo.Common.Models;

namespace IdentityServer.Services
{
    public interface IUserStore
    {
        Task<int> GetTypeAsync(string userEmail);
        Task<IdentityUser> GetUserById(Guid id);
        Task<IdentityUser> GetUserByUsername(string username);
        Task<bool> Update(User user);
        Task<bool> UpdateAsync(User user);
        Task CreateUser(User user);

        IQueryable<User> GetAllUsers(string searchText);
        Task SaveChangesAsync();

    }
}
