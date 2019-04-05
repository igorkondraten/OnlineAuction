using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.BLL.DTO
{
    public class UserDTO
    {
        public int UserProfileId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public UserAddressDTO Address { get; set; }
        public IEnumerable<LotDTO> Lots { get; set; }
        public IEnumerable<BidDTO> Bids { get; set; }
    }
}
