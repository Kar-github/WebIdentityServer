using IdentityServer.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApiDemo.Common.Models;

namespace IdentityServer.Services
{
    public interface IUserStore
    {
        Task<ApplicationUser> GetUserById(Guid id);
        Task<ApplicationUser> GetUserByUsername(string username);
        Task<bool> Update(ApplicationUser user);
        Task<bool> UpdateAsync(ApplicationUser user);
        Task CreateUser(ApplicationUser user);

        Task<IQueryable<ApplicationUser>> GetAllUsers();
    }
}
