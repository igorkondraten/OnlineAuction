namespace OnlineAuction.DAL.Entities
{
    public class UserAddress
    {
        public int UserAddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }

        public virtual UserProfile User { get; set; }
    }
}
