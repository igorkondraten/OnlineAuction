using System;
using System.Collections.Generic;

namespace OnlineAuction.DAL.Entities
{
    public class Lot
    {
        public int LotId { get; set; }
        public string Name { get; set; }
        public decimal InitialPrice { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
