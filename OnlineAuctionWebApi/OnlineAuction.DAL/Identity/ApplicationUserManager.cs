using Microsoft.AspNet.Identity;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IUserManager
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
            this.UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
            };
        }
    }
}
