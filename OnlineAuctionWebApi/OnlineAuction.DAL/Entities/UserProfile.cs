using System;
using System.Collections.Generic;

namespace OnlineAuction.DAL.Entities
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual UserAddress Address { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Lot> Lots { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
