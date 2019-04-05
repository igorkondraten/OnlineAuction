using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.DAL.Interfaces
{
    public interface IUserManager
    {
        IQueryable<ApplicationUser> Users { get; }
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authType);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindAsync(string userName, string password);
        Task<IList<string>> GetRolesAsync(string userId);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task<IdentityResult> DeleteAsync(ApplicationUser user);
    }
}
