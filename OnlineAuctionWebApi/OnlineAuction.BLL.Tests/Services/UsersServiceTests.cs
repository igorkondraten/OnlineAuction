using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.BLL.Services;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.BLL.Tests.Services
{
    [TestFixture]
    public class UsersServiceTests
    {
        private IUsersService _service;
        private Mock<IUnitOfWork> _mockUnitWork;
        private List<UserProfile> _userProfiles;

        [OneTimeSetUp]
        public void Init()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _service = new UsersService(_mockUnitWork.Object);
            _userProfiles = new List<UserProfile>
            {
                new UserProfile(){
                    UserProfileId = 1,
                    Name = "User1",
                    Lots = new List<Lot>(),
                    ApplicationUser = new ApplicationUser()
                    {
                        Id = "", UserName = "User1", Roles = { new IdentityUserRole() }
                    }
                },
                new UserProfile(){
                    UserProfileId = 2,
                    Name = "User2",
                    Lots = new List<Lot>(),
                    ApplicationUser = new ApplicationUser()
                    {
                        Id = "", UserName = "User2", Roles = { new IdentityUserRole() }
                    }
                }
            };
        }

        [Test]
        public async Task CreateUserAsync_CreateUserWithValidData_NewUserReturned()
        {
            var password = "123456";
            var newUser = new UserDTO() { Email = "123@dd.ru", Name = "New user", UserProfileId = 3, Role = "User" };
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);
            _mockUnitWork.Setup(x => x.UserManager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);
            _mockUnitWork.Setup(x => x.RoleManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationRole() { Name = "User" });
            _mockUnitWork.Setup(x => x.UserManager.CreateAsync(It.IsAny<ApplicationUser>(), password))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.GetRolesAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<string>() { "User" });

            var result = await _service.CreateUserAsync(newUser, password);

            Assert.That(result, Is.Not.Null);
            Assert.That(newUser.Name, Is.EqualTo(result.Name));
            Assert.That(newUser.Role, Is.EqualTo(result.Role));
            _mockUnitWork.Verify(x => x.UserManager.CreateAsync(It.IsAny<ApplicationUser>(), password));
        }

        [Test]
        public async Task CreateUserAsync_CreateUserWithDuplicateName_ValidationExceptionThrown()
        {
            var password = "123456";
            var newUser = new UserDTO() { Email = "123@dd.ru", Name = "New user", UserProfileId = 3, Role = "User" };
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            Assert.ThrowsAsync<ValidationException>(() => _service.CreateUserAsync(newUser, password), "User with this name already exists.");
        }

        [Test]
        public async Task CreateUserAsync_CreateUserWithDuplicateEmail_ValidationExceptionThrown()
        {
            var password = "123456";
            var newUser = new UserDTO() { Email = "123@dd.ru", Name = "New user", UserProfileId = 3, Role = "User" };
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);
            _mockUnitWork.Setup(x => x.UserManager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            Assert.ThrowsAsync<ValidationException>(() => _service.CreateUserAsync(newUser, password), "User with this email already exists.");
        }

        [Test]
        public async Task CreateUserAsync_CreateUserWithInvalidRole_CreatedWithUserRole()
        {
            var password = "123456";
            var newUser = new UserDTO() { Email = "123@dd.ru", Name = "New user", UserProfileId = 3, Role = "-" };
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);
            _mockUnitWork.Setup(x => x.UserManager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);
            _mockUnitWork.Setup(x => x.RoleManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationRole)null);
            _mockUnitWork.Setup(x => x.UserManager.CreateAsync(It.IsAny<ApplicationUser>(), password))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.GetRolesAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<string>() { "User" });

            var result = await _service.CreateUserAsync(newUser, password);

            Assert.That(result, Is.Not.Null);
            Assert.That(newUser.Name, Is.EqualTo(result.Name));
            Assert.That(newUser.Role, Is.EqualTo("User"));
            _mockUnitWork.Verify(x => x.UserManager.CreateAsync(It.IsAny<ApplicationUser>(), password));
        }

        [Test]
        public async Task AuthenticateUserAsync_AuthenticateUserWithValidNameAndPassword_ClaimsReturned()
        {
            var username = "User";
            var password = "Password";
            _mockUnitWork.Setup(x => x.UserManager.FindAsync(username, password)).ReturnsAsync(new ApplicationUser());
            _mockUnitWork.Setup(x => x.UserManager.CreateIdentityAsync(It.IsAny<ApplicationUser>(), "Bearer")).ReturnsAsync(new ClaimsIdentity());

            var result = await _service.AuthenticateUserAsync(username, password);

            Assert.That(result, Is.Not.Null);
            _mockUnitWork.Verify(x => x.UserManager.CreateIdentityAsync(It.IsAny<ApplicationUser>(), "Bearer"));
        }

        [Test]
        public async Task GetUserByNameAsync_GetExistingUser_UserReturned()
        {
            var name = "User1";
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser()
                {
                    UserName = name,
                    UserProfile = _userProfiles[0],
                    Email = "",
                    Roles = { new IdentityUserRole() }
                });
            _mockUnitWork.Setup(x => x.UserManager.GetRolesAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<string>() { "User" });

            var result = await _service.GetUserByNameAsync(name);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task GetUserByNameAsync_GetNotExistingUser_NotFoundExceptionThrown()
        {
            var name = "User1";
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserByNameAsync(name), "Username is invalid.");
        }

        [Test]
        public async Task GetAllUsersAsync_GetAllUsers_UsersReturned()
        {
            var limit = 10;
            var offset = 0;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAllAsync(limit, offset)).ReturnsAsync((Items: _userProfiles, TotalCount: _userProfiles.Count));
            _mockUnitWork.Setup(x => x.RoleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationRole() { Name = "User" });

            var results = await _service.GetAllUsersAsync(limit, offset);

            Assert.That(results.Users, Is.Not.Null);
            Assert.That(results.Users.Count(), Is.EqualTo(2));
            Assert.That(results.Users.Select(x => x.Role), Is.EqualTo(new List<string>() { "User", "User" }));
        }

        [Test]
        public async Task UpdateUserAsync_UpdateUserWithoutChangingRole_UserUpdated()
        {
            var newUser = new UserDTO() { Email = "123@dd.ru", Name = "New user", UserProfileId = 3, Role = "User" };
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_userProfiles[0].ApplicationUser);
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(newUser.UserProfileId)).ReturnsAsync(_userProfiles[0]);
            _mockUnitWork.Setup(x => x.RoleManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationRole() { Name = "User" });
            _mockUnitWork.Setup(x => x.UserManager.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.GetRolesAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<string>() { "User" });
            _mockUnitWork.Setup(x => x.UserManager.RemoveFromRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new IdentityResult());

            await _service.UpdateUserAsync(newUser);

            _mockUnitWork.Verify(x => x.UserManager.UpdateAsync(It.IsAny<ApplicationUser>()));
        }

        [Test]
        public async Task UpdateUserAsync_UpdateUserWithChangingRole_UserUpdated()
        {
            var newUser = new UserDTO() { Email = "123@dd.ru", Name = "New user", UserProfileId = 3, Role = "Admin" };
            _mockUnitWork.Setup(x => x.UserManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_userProfiles[0].ApplicationUser);
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(newUser.UserProfileId)).ReturnsAsync(_userProfiles[0]);
            _mockUnitWork.Setup(x => x.RoleManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationRole() { Name = "User" });
            _mockUnitWork.Setup(x => x.UserManager.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.UserManager.GetRolesAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<string>() { "User" });
            _mockUnitWork.Setup(x => x.UserManager.RemoveFromRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new IdentityResult());

            await _service.UpdateUserAsync(newUser);

            _mockUnitWork.Verify(x => x.UserManager.UpdateAsync(It.IsAny<ApplicationUser>()));
            _mockUnitWork.Verify(x => x.UserManager.AddToRoleAsync(It.IsAny<string>(), "Admin"));
        }

        [Test]
        public async Task GetUserByProfileAsync_GetExistingUser_UserReturned()
        {
            var profileId = 1;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(profileId)).ReturnsAsync(_userProfiles[0]);
            _mockUnitWork.Setup(x => x.RoleManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationRole() { Name = "User" });

            var result = await _service.GetUserByProfileAsync(profileId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Role, Is.EqualTo("User"));
        }

        [Test]
        public async Task GetUserByProfileAsync_GetNotExistingUser_NotFoundExceptionThrown()
        {
            var profileId = 1;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(profileId)).ReturnsAsync((UserProfile)null);

            Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserByProfileAsync(profileId));
        }

        [Test]
        public async Task UpdateUserPasswordAsync_PasswordIsValid_PasswordChanged()
        {
            var oldPassword = "123456";
            var newPassword = "654321";
            var profileId = 1;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(profileId)).ReturnsAsync(_userProfiles[0]);
            _mockUnitWork
                .Setup(x => x.UserManager.ChangePasswordAsync(_userProfiles[0].ApplicationUser.Id, oldPassword,
                    newPassword)).ReturnsAsync(new IdentityResult());

            await _service.UpdateUserPasswordAsync(profileId, oldPassword, newPassword);

            _mockUnitWork.Verify(x => x.UserManager.ChangePasswordAsync(_userProfiles[0].ApplicationUser.Id, oldPassword,
                newPassword));
        }

        [Test]
        public async Task UpdateUserPasswordAsync_UpdatePasswordOfUserNotExists_NotFoundExceptionThrown()
        {
            var oldPassword = "123456";
            var newPassword = "654321";
            var profileId = 1;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(profileId)).ReturnsAsync((UserProfile)null);

            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateUserPasswordAsync(profileId, oldPassword, newPassword));
        }

        [Test]
        public async Task UpdateUserPasswordAsync_InvalidOldPassword_ValidationExceptionThrown()
        {
            var oldPassword = "123456";
            var newPassword = "654321";
            var profileId = 1;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(profileId)).ReturnsAsync(_userProfiles[0]);
            _mockUnitWork
                .Setup(x => x.UserManager.ChangePasswordAsync(_userProfiles[0].ApplicationUser.Id, oldPassword,
                    newPassword)).ReturnsAsync(new IdentityResult("Invalid password"));

            Assert.ThrowsAsync<ValidationException>(() => _service.UpdateUserPasswordAsync(profileId, oldPassword, newPassword));
        }

        [Test]
        public async Task DeleteUserAsync_DeleteUserWithoutCreatedLots_UserDeleted()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(id)).ReturnsAsync(_userProfiles[0]);
            _mockUnitWork.Setup(x => x.UserProfiles.Delete(id));
            _mockUnitWork.Setup(x => x.UserManager.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new IdentityResult());
            _mockUnitWork.Setup(x => x.Lots.Delete(It.IsAny<int>()));

            await _service.DeleteUserAsync(id);

            _mockUnitWork.Verify(x => x.UserProfiles.Delete(id));
            _mockUnitWork.Verify(x => x.UserManager.DeleteAsync(It.IsAny<ApplicationUser>()));
        }

        [Test]
        public async Task DeleteUserAsync_DeleteUserWithCreatedLots_UserAndLotsDeleted()
        {
            var user = new UserProfile()
            {
                UserProfileId = 2,
                Name = "User2",
                Lots = new List<Lot>() { new Lot(), new Lot() },
                ApplicationUser = new ApplicationUser()
                {
                    UserName = "User2",
                    Roles = { new IdentityUserRole() }
                }
            };
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(user.UserProfileId)).ReturnsAsync(_userProfiles[0]);
            _mockUnitWork.Setup(x => x.UserProfiles.Delete(user.UserProfileId));
            _mockUnitWork.Setup(x => x.UserManager.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new IdentityResult());

            await _service.DeleteUserAsync(user.UserProfileId);

            _mockUnitWork.Verify(x => x.UserProfiles.Delete(user.UserProfileId));
            _mockUnitWork.Verify(x => x.UserManager.DeleteAsync(It.IsAny<ApplicationUser>()));
            _mockUnitWork.Setup(x => x.Lots.Delete(It.IsAny<int>()));
        }

        [Test]
        public async Task DeleteUserAsync_DeleteNotExistingUser_NotFoundExceptionThrown()
        {
            var id = 10;
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(id)).ReturnsAsync((UserProfile)null);

            Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteUserAsync(id));
        }

    }
}