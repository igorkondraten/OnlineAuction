using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.DAL.EF
{
    public class AuctionContext : IdentityDbContext<ApplicationUser>, IDataContext
    {
        static AuctionContext()
        {
            Database.SetInitializer<AuctionContext>(new AuctionContextInitializer());
        }

        public AuctionContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserAddress>().HasRequired(x => x.User).WithOptional(x => x.Address).WillCascadeOnDelete(true);
            modelBuilder.Entity<UserProfile>().HasRequired(x => x.ApplicationUser).WithOptional(x => x.UserProfile);
            modelBuilder.Entity<Category>().HasMany(p => p.Lots).WithRequired(p => p.Category);
            modelBuilder.Entity<Lot>().HasMany(p => p.Bids).WithRequired(p => p.Lot).WillCascadeOnDelete(true);
            modelBuilder.Entity<UserProfile>().HasMany(p => p.Lots).WithRequired(p => p.User).HasForeignKey(p => p.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserProfile>().HasMany(p => p.Bids).WithRequired(p => p.PlacedUser).HasForeignKey(p => p.PlacedUserId);
            modelBuilder.Entity<Category>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Lot>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Lot>().Property(p => p.Description).HasMaxLength(1000).IsRequired();
            modelBuilder.Entity<UserAddress>().Property(p => p.City).HasMaxLength(50);
            modelBuilder.Entity<UserAddress>().Property(p => p.Country).HasMaxLength(100);
            modelBuilder.Entity<UserAddress>().Property(p => p.Street).HasMaxLength(200);
            modelBuilder.Entity<UserAddress>().Property(p => p.ZipCode).HasMaxLength(18);
            modelBuilder.Entity<UserProfile>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        }

        public IDbSet<Bid> Bids { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Lot> Lots { get; set; }
        public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<UserAddress> UserAddresses { get; set; }
    }
}
