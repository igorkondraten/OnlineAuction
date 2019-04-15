using System.ComponentModel.DataAnnotations;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.API.Models
{
    /// <summary>
    /// New user registration model.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// The user name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The user password.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>
        /// The user email.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The user address.
        /// </summary>
        public UserAddressDTO Address { get; set; }
    }
}