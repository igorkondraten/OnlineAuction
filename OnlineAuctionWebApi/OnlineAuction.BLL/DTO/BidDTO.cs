using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.BLL.DTO
{
    /// <summary>
    /// Bid data transfer object which contains information about the bid.
    /// </summary>
    public class BidDTO
    {
        /// <summary>
        /// Id of the bid.
        /// </summary>
        public int BidId { get; set; }

        /// <summary>
        /// Price of the bid.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Date when bid was placed.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Name of the user who placed the bid.
        /// </summary>
        public string PlacedUserName { get; set; }

        /// <summary>
        /// Id of the lot on which bid was placed.
        /// </summary>
        public int LotId { get; set; }
    }
}
