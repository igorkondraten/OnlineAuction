using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.API.Models
{
    /// <summary>
    /// Model for updating password of user.
    /// </summary>
    public class UpdateUserPasswordModel
    {
        /// <summary>
        /// Old user password.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string OldPassword { get; set; }

        /// <summary>
        /// New user password.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}