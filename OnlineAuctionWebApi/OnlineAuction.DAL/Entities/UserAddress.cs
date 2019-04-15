namespace OnlineAuction.DAL.Entities
{
    /// <summary>
    /// Address of the user entity.
    /// </summary>
    public class UserAddress
    {
        /// <summary>
        /// Id of the user address.
        /// </summary>
        public int UserAddressId { get; set; }

        /// <summary>
        /// Country of the user.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// City of the user.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Zip code of the user address.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Street of the user address.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// User profile.
        /// </summary>
        public virtual UserProfile User { get; set; }
    }
}
