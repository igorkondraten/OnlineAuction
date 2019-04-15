using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OnlineAuction.BLL.Enums;

namespace OnlineAuction.BLL.DTO
{
    /// <summary>
    /// Lot data transfer object which contains information about the lot.
    /// </summary>
    public class LotDTO
    {
        /// <summary>
        /// Id of the lot.
        /// </summary>
        public int LotId { get; set; }

        /// <summary>
        /// Name of the lot.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Initial price of the lot.
        /// </summary>
        [Required]
        public decimal InitialPrice { get; set; }

        /// <summary>
        /// Date when the auction is starting.
        /// </summary>
        [Required]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Date when the auction is ending.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Description of the lot.
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Current lot status.
        /// </summary>
        public AuctionStatus Status { get; set; }

        /// <summary>
        /// Bid with the best price at this moment.
        /// </summary>
        public BidDTO BestBid { get; set; }

        /// <summary>
        /// Current maximum price.
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Path to photo of the lot.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Byte array of the lot's photo.
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Name of the user who created the lot.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Category where the lot was created.
        /// </summary>
        [Required]
        public CategoryDTO Category { get; set; }

        /// <summary>
        /// Bids placed on the lot.
        /// </summary>
        public IEnumerable<BidDTO> Bids { get; set; }
    }
}
