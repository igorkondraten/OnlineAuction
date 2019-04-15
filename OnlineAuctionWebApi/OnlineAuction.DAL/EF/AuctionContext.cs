using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.DAL.EF
{
    /// <summary>
    /// Database context of the application.
    /// </summary>
    public class AuctionContext : IdentityDbContext<ApplicationUser>, IDataContext
    {
        /// <summary>
        /// Static constructor which sets database initializer.
        /// </summary>
        static AuctionContext()
        {
            Database.SetInitializer<AuctionContext>(new AuctionContextInitializer());
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
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

        /// <summary>
        /// DbSet of bid entities.
        /// </summary>
        public IDbSet<Bid> Bids { get; set; }

        /// <summary>
        /// DbSet of bid entities.
        /// </summary>
        public IDbSet<Category> Categories { get; set; }

        /// <summary>
        /// DbSet of lot entities.
        /// </summary>
        public IDbSet<Lot> Lots { get; set; }

        /// <summary>
        /// DbSet of user profile entities.
        /// </summary>
        public IDbSet<UserProfile> UserProfiles { get; set; }

        /// <summary>
        /// DbSet of user address entities.
        /// </summary>
        public IDbSet<UserAddress> UserAddresses { get; set; }
    }
}
