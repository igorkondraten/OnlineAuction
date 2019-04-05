using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.API.Models
{
    public class UpdateUserPasswordModel
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}