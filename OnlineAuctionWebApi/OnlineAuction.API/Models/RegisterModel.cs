using System.ComponentModel.DataAnnotations;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.API.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public UserAddressDTO Address { get; set; }
    }
}