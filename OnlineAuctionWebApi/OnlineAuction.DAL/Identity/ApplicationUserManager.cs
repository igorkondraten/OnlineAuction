using Microsoft.AspNet.Identity;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.DAL.Identity
{
    /// <summary>
    /// Manager for identity users.
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>, IUserManager
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
            };
        }
    }
}
