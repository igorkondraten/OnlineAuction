using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OnlineAuction.BLL.Enums;

namespace OnlineAuction.BLL.DTO
{
    public class LotDTO
    {
        public int LotId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public decimal InitialPrice { get; set; }
        [Required]
        public DateTime BeginDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        public AuctionStatus Status { get; set; }
        public BidDTO BestBid { get; set; }
        public decimal CurrentPrice { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }
        public string UserName { get; set; }
        [Required]
        public CategoryDTO Category { get; set; }
        public IEnumerable<BidDTO> Bids { get; set; }
    }
}
