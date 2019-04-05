using System;
using System.Threading.Tasks;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces.Repositories;

namespace OnlineAuction.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserManager UserManager { get; }
        IRoleManager RoleManager { get; }
        IBidsRepository Bids { get; }
        ICategoriesRepository Categories { get; }
        ILotsRepository Lots { get; }
        IUserAddressesRepository UserAddresses { get; }
        IUserProfilesRepository UserProfiles { get; }
        void Save();
        Task SaveAsync();
    }
}
