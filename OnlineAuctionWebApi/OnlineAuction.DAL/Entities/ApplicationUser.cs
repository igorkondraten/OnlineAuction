using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineAuction.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile UserProfile { get; set; }
    }
}
