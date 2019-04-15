using System;

namespace OnlineAuction.DAL.Entities
{
    /// <summary>
    /// Bid entity.
    /// </summary>
    public class Bid
    {
        /// <summary>
        /// Id of the bid.
        /// </summary>
        public int BidId { get; set; }

        /// <summary>
        /// Id of the user profile who placed bid.
        /// </summary>
        public int PlacedUserId { get; set; }

        /// <summary>
        /// Id of the lot where the bid was placed.
        /// </summary>
        public int LotId { get; set; }

        /// <summary>
        /// The bid price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Date when the bid was placed.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// User who placed bid.
        /// </summary>
        public virtual UserProfile PlacedUser { get; set; }

        /// <summary>
        /// Lot where the bid was placed.
        /// </summary>
        public virtual Lot Lot { get; set; }
    }
}
