using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.BLL.DTO
{
    /// <summary>
    /// User address data transfer object which contains information about the user's address.
    /// </summary>
    public class UserAddressDTO
    {
        /// <summary>
        /// Country of the user.
        /// </summary>
        [MaxLength(100)]
        public string Country { get; set; }

        /// <summary>
        /// City of the user.
        /// </summary>
        [MaxLength(50)]
        public string City { get; set; }

        /// <summary>
        /// Zip code of the user address.
        /// </summary>
        [MaxLength(18)]
        public string ZipCode { get; set; }

        /// <summary>
        /// Street of the user address.
        /// </summary>
        [MaxLength(200)]
        public string Street { get; set; }
    }
}
