using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.BLL.DTO
{
    public class BidDTO
    {
        public int BidId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string PlacedUserName { get; set; }
        public int LotId { get; set; }
    }
}
