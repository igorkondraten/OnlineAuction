using System.Data.Entity;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.DAL.EF
{
    /// <summary>
    /// Context initializer.
    /// </summary>
    internal class AuctionContextInitializer : DropCreateDatabaseIfModelChanges<AuctionContext>
    {
        protected override void Seed(AuctionContext db)
        {
            ApplicationRole admin = new ApplicationRole() {Name = "Admin"};
            ApplicationRole user = new ApplicationRole() { Name = "User" };
            ApplicationRole seller = new ApplicationRole() { Name = "Seller" };
            db.Roles.Add(admin);
            db.Roles.Add(user);
            db.Roles.Add(seller);
        }
    }
}
