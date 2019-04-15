using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.BLL.DTO
{
    /// <summary>
    /// User data transfer object which contains information about the user.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Id of the user profile.
        /// </summary>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Role of the user.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Registration date of the user.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Address of the user.
        /// </summary>
        public UserAddressDTO Address { get; set; }

        /// <summary>
        /// Lots created by the user.
        /// </summary>
        public IEnumerable<LotDTO> Lots { get; set; }

        /// <summary>
        /// Bids placed by the user.
        /// </summary>
        public IEnumerable<BidDTO> Bids { get; set; }
    }
}
