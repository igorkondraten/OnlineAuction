using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OnlineAuction.DAL.Interfaces;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Identity;
using OnlineAuction.DAL.Interfaces.Repositories;

namespace OnlineAuction.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDataContext _context;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IRoleStore<ApplicationRole, string> _roleStore;
        private IUserManager _userManager;
        private IRoleManager _roleManager;
        private IBidsRepository _bidsRepository;
        private ICategoriesRepository _categoriesRepository;
        private ILotsRepository _lotsRepository;
        private IUserAddressesRepository _userAddressesRepository;
        private IUserProfilesRepository _userProfilesRepository;

        public IUserManager UserManager => _userManager ?? (_userManager = new ApplicationUserManager(_userStore));
        public IRoleManager RoleManager => _roleManager ?? (_roleManager = new ApplicationRoleManager(_roleStore));
        public IBidsRepository Bids => _bidsRepository ?? (_bidsRepository = new BidsRepository(_context));
        public ICategoriesRepository Categories => _categoriesRepository ?? (_categoriesRepository = new CategoriesRepository(_context));
        public ILotsRepository Lots => _lotsRepository ?? (_lotsRepository = new LotsRepository(_context));
        public IUserAddressesRepository UserAddresses => _userAddressesRepository ?? (_userAddressesRepository = new UserAddressesRepository(_context));
        public IUserProfilesRepository UserProfiles => _userProfilesRepository ?? (_userProfilesRepository = new UserProfilesRepository(_context));
        
        public UnitOfWork(IDataContext context, IUserStore<ApplicationUser> userStore, IRoleStore<ApplicationRole, string> roleStore)
        {
            _context = context;
            _userStore = userStore;
            _roleStore = roleStore;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
