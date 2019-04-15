using System;
using System.Threading.Tasks;
using OnlineAuction.DAL.Interfaces.Repositories;

namespace OnlineAuction.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Interface for accessing database by repositories.
        /// </summary>
        IUserManager UserManager { get; }

        /// <summary>
        /// Gets identity role manager.
        /// </summary>
        IRoleManager RoleManager { get; }

        /// <summary>
        /// Gets bids repository.
        /// </summary>
        IBidsRepository Bids { get; }

        /// <summary>
        /// Gets category repository.
        /// </summary>
        ICategoriesRepository Categories { get; }

        /// <summary>
        /// Gets lots repository.
        /// </summary>
        ILotsRepository Lots { get; }

        /// <summary>
        /// Gets user addresses repository.
        /// </summary>
        IUserAddressesRepository UserAddresses { get; }

        /// <summary>
        /// Gets user profiles repository.
        /// </summary>
        IUserProfilesRepository UserProfiles { get; }

        /// <summary>
        /// Method for saving db changes.
        /// </summary>
        void Save();

        /// <summary>
        /// Async method for saving db changes.
        /// </summary>
        /// <returns>The Task.</returns>
        Task SaveAsync();
    }
}
