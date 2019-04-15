using System;
using System.Collections.Generic;

namespace OnlineAuction.DAL.Entities
{
    /// <summary>
    /// User profile entity which contains information about the user.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Id of the user profile.
        /// </summary>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date when user was registered.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Address of the user.
        /// </summary>
        public virtual UserAddress Address { get; set; }

        /// <summary>
        /// Link to identity user.
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// Lots created by the user.
        /// </summary>
        public virtual ICollection<Lot> Lots { get; set; }

        /// <summary>
        /// Bids placed by the user.
        /// </summary>
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
