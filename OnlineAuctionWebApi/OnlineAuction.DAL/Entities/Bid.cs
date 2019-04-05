using System;

namespace OnlineAuction.DAL.Entities
{
    public class Bid
    {
        public int BidId { get; set; }
        public int PlacedUserId { get; set; }
        public int LotId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public virtual UserProfile PlacedUser { get; set; }
        public virtual Lot Lot { get; set; }
    }
}
