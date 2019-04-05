using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.BLL.Services
{
    public class UsersService : IUsersService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User is null.");
            if (await _unitOfWork.UserManager.FindByNameAsync(user.Name) != null)
                throw new ValidationException("User with this name already exists.");
            if (await _unitOfWork.UserManager.FindByEmailAsync(user.Email) != null)
                throw new ValidationException("User with this email already exists.");
            if (string.IsNullOrEmpty(user.Role) || await _unitOfWork.RoleManager.FindByNameAsync(user.Role) == null)
                user.Role = "User";
            var appUser = Mapper.Map<UserDTO, ApplicationUser>(user);
            var userProfile = new UserProfile()
            {
                ApplicationUser = appUser,
                RegistrationDate = DateTime.UtcNow,
                Name = appUser.UserName,
                Address = Mapper.Map<UserAddressDTO, UserAddress>(user.Address)
            };
            appUser.UserProfile = userProfile;
            var result = await _unitOfWork.UserManager.CreateAsync(appUser, password);
            if (result.Errors.Any())
                throw new ValidationException(string.Join(", ", result.Errors));
            await _unitOfWork.UserManager.AddToRoleAsync(appUser.Id, user.Role);
            var createdUser = Mapper.Map<UserProfile, UserDTO>(userProfile);
            createdUser.Role = (await _unitOfWork.UserManager.GetRolesAsync(appUser.Id)).FirstOrDefault();
            return createdUser;
        }

        public async Task<ClaimsIdentity> AuthenticateUserAsync(string userName, string password)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await _unitOfWork.UserManager.FindAsync(userName, password);
            if (user != null)
                claim = await _unitOfWork.UserManager.CreateIdentityAsync(user, "Bearer");
            return claim;
        }

        public async Task<UserDTO> GetUserByNameAsync(string name)
        {
            var appUser = await _unitOfWork.UserManager.FindByNameAsync(name);
            if (appUser == null)
                throw new NotFoundException("Username is invalid.");
            var userRole = await _unitOfWork.UserManager.GetRolesAsync(appUser.Id);
            var user = Mapper.Map<ApplicationUser, UserDTO>(appUser);
            user.Role = userRole.FirstOrDefault();
            return user;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            return new UserDTO() { Name = user.UserName };
        }

        public async Task<(IEnumerable<UserDTO> Users, int TotalCount)> GetAllUsersAsync(int limit, int offset)
        {
            var (users, totalCount) = await _unitOfWork.UserProfiles.GetAllAsync(limit, offset);
            var usersDto = users.Select(x => Mapper.Map<UserProfile, UserDTO>(x)).ToList();
            // Setting user's role name instead of id.
            for (var i = 0; i < usersDto.Count(); i++)
            {
                var role = await _unitOfWork.RoleManager.FindByIdAsync(usersDto[i].Role);
                if (role != null)
                {
                    usersDto[i].Role = role.Name;
                }
            }
            return (usersDto, totalCount);
        }

        public async Task UpdateUserAsync(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User is null.");
            var oldUser = await _unitOfWork.UserProfiles.GetAsync(user.UserProfileId);
            if (oldUser == null)
                throw new NotFoundException("User not found.");
            if (string.IsNullOrEmpty(user.Role) || await _unitOfWork.RoleManager.FindByNameAsync(user.Role) == null)
                throw new ArgumentException("Invalid role.");
            var appUser = await _unitOfWork.UserManager.FindByNameAsync(oldUser.Name);
            appUser.Email = user.Email;
            appUser.UserName = user.Name;
            var result = await _unitOfWork.UserManager.UpdateAsync(appUser);
            if (result.Errors.Any())
                throw new ValidationException(string.Join(", ", result.Errors));
            var currentRole = (await _unitOfWork.UserManager.GetRolesAsync(appUser.Id)).FirstOrDefault();
            if (currentRole != user.Role)
            {
                await _unitOfWork.UserManager.RemoveFromRoleAsync(appUser.Id, user.Role);
                await _unitOfWork.UserManager.AddToRoleAsync(appUser.Id, user.Role);
            }
            user.RegistrationDate = oldUser.RegistrationDate;
            _unitOfWork.UserProfiles.Update(Mapper.Map<UserDTO, UserProfile>(user, oldUser));
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserDTO> GetUserByProfileAsync(int profileId)
        {
            var user = await _unitOfWork.UserProfiles.GetAsync(profileId);
            if (user == null)
                throw new NotFoundException("User not found");
            var userDto = Mapper.Map<UserProfile, UserDTO>(user);
            userDto.Role = (await _unitOfWork.RoleManager.FindByIdAsync(userDto.Role))?.Name;
            return userDto;
        }

        public async Task UpdateUserPasswordAsync(int profileId, string oldPassword, string newPassword)
        {
            var user = await _unitOfWork.UserProfiles.GetAsync(profileId);
            if (user == null)
                throw new NotFoundException("User not found");
            var result =
                await _unitOfWork.UserManager.ChangePasswordAsync(user.ApplicationUser.Id, oldPassword, newPassword);
            if (result.Errors.Any())
                throw new ValidationException(string.Join(", ", result.Errors));
        }

        public async Task DeleteUserAsync(int profileId)
        {
            var userProfile = await _unitOfWork.UserProfiles.GetAsync(profileId);
            if (userProfile == null)
                throw new NotFoundException("User not found.");
            var appUser = userProfile.ApplicationUser;
            _unitOfWork.UserProfiles.Delete(profileId);
            var result = await _unitOfWork.UserManager.DeleteAsync(appUser);
            if (result.Errors.Any())
                throw new DataException(string.Join(", ", result.Errors));
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
