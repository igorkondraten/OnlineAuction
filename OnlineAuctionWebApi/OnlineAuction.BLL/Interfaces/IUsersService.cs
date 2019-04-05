using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    public interface IUsersService : IDisposable
    {
        Task<UserDTO> CreateUserAsync(UserDTO user, string password);
        Task UpdateUserAsync(UserDTO user);
        Task UpdateUserPasswordAsync(int profileId, string oldPassword, string newPassword);
        Task DeleteUserAsync(int profileId);
        Task<ClaimsIdentity> AuthenticateUserAsync(string userName, string password);
        Task<UserDTO> GetUserByNameAsync(string name);
        Task<UserDTO> GetUserByProfileAsync(int profileId);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<(IEnumerable<UserDTO> Users, int TotalCount)> GetAllUsersAsync(int limit, int offset);
    }
}
