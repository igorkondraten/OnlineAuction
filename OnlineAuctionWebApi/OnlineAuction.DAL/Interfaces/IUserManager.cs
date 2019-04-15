using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.DAL.Interfaces
{
    /// <summary>
    /// Interface for identity user manager.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Creates new user with password.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The Task, containing the IdentityResult of the operation.</returns>
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        /// <summary>
        /// Creates a ClaimsIdentity representing the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="authType">The authentication type.</param>
        /// <returns>The Task, containing ClaimsIdentity.</returns>
        Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authType);

        /// <summary>
        /// Finds a user by his e-mail.
        /// </summary>
        /// <param name="email">The e-mail.</param>
        /// <returns>The Task, containing user.</returns>
        Task<ApplicationUser> FindByEmailAsync(string email);

        /// <summary>
        /// Returns a user with the specified username and password or null if there is no match.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The Task, containing user.</returns>
        Task<ApplicationUser> FindAsync(string userName, string password);

        /// <summary>
        /// Returns the roles for the user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The Task, containing list of role names.</returns>
        Task<IList<string>> GetRolesAsync(string userId);

        /// <summary>
        /// Adds a user to a role.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="role">The role.</param>
        /// <returns>The Task, containing the IdentityResult of the operation.</returns>
        Task<IdentityResult> AddToRoleAsync(string userId, string role);

        /// <summary>
        /// Removes a user from a role.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="role">The role.</param>
        /// <returns>The Task, containing the IdentityResult of the operation.</returns>
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);

        /// <summary>
        /// Finds a user by user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The Task, containing user.</returns>
        Task<ApplicationUser> FindByNameAsync(string userName);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The Task, containing the IdentityResult of the operation.</returns>
        Task<IdentityResult> UpdateAsync(ApplicationUser user);

        /// <summary>
        /// Changes a user password.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="oldPassword">The current password used.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>The Task, containing the IdentityResult of the operation.</returns>
        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The Task, containing the IdentityResult of the operation.</returns>
        Task<IdentityResult> DeleteAsync(ApplicationUser user);
    }
}