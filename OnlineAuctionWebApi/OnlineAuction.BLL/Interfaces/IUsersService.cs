using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    /// <summary>
    /// Interface for users services.
    /// Contains methods for managing users and their profiles.
    /// </summary>
    public interface IUsersService : IDisposable
    {
        /// <summary>
        /// Async method for creating new user with password.
        /// </summary>
        /// <param name="user">The user DTO.</param>
        /// <param name="password">The password.</param>
        /// <returns>The Task, containing user DTO.</returns>
        Task<UserDTO> CreateUserAsync(UserDTO user, string password);

        /// <summary>
        /// Async method for updating user and his profile.
        /// </summary>
        /// <param name="user">The user DTO.</param>
        /// <returns>The Task.</returns>
        Task UpdateUserAsync(UserDTO user);

        /// <summary>
        /// Async method for updating user password.
        /// </summary>
        /// <param name="profileId">The profile ID.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>The Task.</returns>
        Task UpdateUserPasswordAsync(int profileId, string oldPassword, string newPassword);

        /// <summary>
        /// Async method for deleting user and his profile. 
        /// </summary>
        /// <param name="profileId">The profile ID.</param>
        /// <returns>The Task.</returns>
        Task DeleteUserAsync(int profileId);

        /// <summary>
        /// Async method for authenticating user.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The Task, containing ClaimsIdentity.</returns>
        Task<ClaimsIdentity> AuthenticateUserAsync(string userName, string password);

        /// <summary>
        /// Async method for finding user by name.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <returns>The Task, containing user DTO.</returns>
        Task<UserDTO> GetUserByNameAsync(string name);

        /// <summary>
        /// Async method for finding user by profile ID.
        /// </summary>
        /// <param name="profileId">The profile ID.</param>
        /// <returns>The Task, containing user DTO.</returns>
        Task<UserDTO> GetUserByProfileAsync(int profileId);

        /// <summary>
        /// Async method that gets all users with pagination.
        /// </summary>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>The Task, containing collection of user DTOs and total users count.</returns>
        Task<(IEnumerable<UserDTO> Users, int TotalCount)> GetAllUsersAsync(int limit, int offset);
    }
}
