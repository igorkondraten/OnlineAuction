using System;
using System.Collections.Generic;

namespace OnlineAuction.DAL.Entities
{
    /// <summary>
    /// Lot entity.
    /// </summary>
    public class Lot
    {
        /// <summary>
        /// Id of the lot.
        /// </summary>
        public int LotId { get; set; }

        /// <summary>
        /// Name of the lot.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initial price of the lot.
        /// </summary>
        public decimal InitialPrice { get; set; }

        /// <summary>
        /// Date when the auction is starting.
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Date when the auction is ending.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Description of the lot.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User Id who created the lot.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Category Id where the lot was created.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Address of the lot photo.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// User who created the lot.
        /// </summary>
        public virtual UserProfile User { get; set; }

        /// <summary>
        /// Category where the lot was created.
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Bids placed on the lot.
        /// </summary>
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
