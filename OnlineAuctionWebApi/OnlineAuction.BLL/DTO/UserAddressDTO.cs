using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.BLL.DTO
{
    public class UserAddressDTO
    {
        [MaxLength(100)]
        public string Country { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(18)]
        public string ZipCode { get; set; }
        [MaxLength(200)]
        public string Street { get; set; }
    }
}
