using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OnlineAuction.API.Models;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Interfaces;

namespace OnlineAuction.API.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        /// Get all users with pagination.
        /// </summary>
        /// <param name="model">Pagination model.</param>
        /// <returns>200 - at least 1 user found; 204 - no users found.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetAllUsersAsync([FromUri] PagingModel model)
        {
            if (model == null || !ModelState.IsValid)
                model = new PagingModel() { Limit = 10, Offset = 0 };
            var (users, totalCount) = await _usersService.GetAllUsersAsync(model.Limit, model.Offset);
            if (!users.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(new { users, totalCount });
        }

        /// <summary>
        /// Get user by profile ID.
        /// </summary>
        /// <param name="profileId">User profile ID.</param>
        /// <returns>200 - user found; 404 - user not found.</returns>
        [HttpGet]
        [Route("{profileId:int:min(1)}")]
        public async Task<IHttpActionResult> GetUserAsync(int profileId)
        {
            var user = await _usersService.GetUserByProfileAsync(profileId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Get current user who is making request.
        /// </summary>
        /// <returns>200 - user found, 404 - user not found.</returns>
        [HttpGet]
        [Route("current")]
        public async Task<IHttpActionResult> GetCurrentUserAsync()
        {
            var user = await _usersService.GetUserByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Register and create new user.
        /// </summary>
        /// <param name="model">Registration model.</param>
        /// <returns>201 - user created, 400 - validation failed.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RegisterUserAsync(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new UserDTO()
            {
                Name = model.Name,
                Email = model.Email,
                Role = "User",
                Address = model.Address
            };
            var createdUser = await _usersService.CreateUserAsync(user, model.Password);
            return Created(Request.RequestUri + "/" + createdUser?.UserProfileId, createdUser);
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="profileId">User profile ID.</param>
        /// <param name="user">User.</param>
        /// <returns>200 - user updated, 403 - authorization failed, 404 - user not found; 400 - validation failed.</returns>
        [HttpPut]
        [Route("{profileId:int:min(1)}")]
        public async Task<IHttpActionResult> EditUserAsync(int profileId, UserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var editedUser = await _usersService.GetUserByProfileAsync(profileId);
            if (editedUser == null)
                return NotFound();
            // Not admin user can edit only his own profile.
            if (!User.IsInRole("Admin"))
            {
                if (editedUser.Name != User.Identity.Name)
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Forbidden));
                // Only admin can change user's role.
                user.Role = editedUser.Role;
            }
            user.UserProfileId = profileId;
            await _usersService.UpdateUserAsync(user);
            return Ok();
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="profileId">User profile ID.</param>
        /// <param name="model">Update password model.</param>
        /// <returns>200 - password changed, 400 - validation failed, 404 - user not found.</returns>
        [HttpPut]
        [Route("{profileId:int:min(1)}/password")]
        public async Task<IHttpActionResult> EditUserPasswordAsync(int profileId, UpdateUserPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _usersService.UpdateUserPasswordAsync(profileId, model.OldPassword, model.NewPassword);
            return Ok();
        }

        /// <summary>
        /// Delete user by profile ID.
        /// </summary>
        /// <param name="profileId">User profile ID.</param>
        /// <returns>404 - user not found; 204 - user deleted.</returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{profileId:int:min(1)}")]
        public async Task<IHttpActionResult> DeleteUserAsync(int profileId)
        {
            await _usersService.DeleteUserAsync(profileId);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
    }
}
