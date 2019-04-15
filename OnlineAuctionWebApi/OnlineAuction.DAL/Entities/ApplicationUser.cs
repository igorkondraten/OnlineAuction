using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineAuction.DAL.Entities
{
    /// <summary>
    /// Application user entity.
    /// Contains base identity properties.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Link to connected user profile.
        /// </summary>
        public virtual UserProfile UserProfile { get; set; }
    }
}
