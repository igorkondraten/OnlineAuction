using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;
using OnlineAuction.DAL.Interfaces.Repositories;

namespace OnlineAuction.DAL.Repositories
{
    /// <summary>
    /// Contains methods for processing entities in UserAddresses table.
    /// </summary>
    public sealed class UserAddressesRepository : IUserAddressesRepository
    {
        private readonly IDataContext _context;

        public UserAddressesRepository(IDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for fetching all user addresses from table.
        /// </summary>
        public IEnumerable<UserAddress> GetAll()
        {
            return _context.Set<UserAddress>().ToList();
        }

        /// <summary>
        /// Method for fetching all user addresses from table with pagination.
        /// </summary>
        public (IEnumerable<UserAddress> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<UserAddress>().OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToList(),
                _context.Set<UserAddress>().Count());
        }

        /// <summary>
        /// Method for fetching user address by id (primary key).
        /// </summary>
        public UserAddress Get(int id)
        {
            return _context.Set<UserAddress>().FirstOrDefault(c => c.UserAddressId == id);
        }

        /// <summary>
        /// Method for finding user addresses by expression with pagination.
        /// </summary>
        public (IEnumerable<UserAddress> Items, int TotalCount) Find(Expression<Func<UserAddress, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<UserAddress>().Where(expression);
            return (query.OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToList(), query.Count());
        }

        /// <summary>
        /// Method for creating user address.
        /// </summary>
        public void Create(UserAddress item)
        {
            _context.Set<UserAddress>().Add(item);
        }

        /// <summary>
        /// Method for updating user address.
        /// </summary>
        public void Update(UserAddress item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting user address.
        /// </summary>
        public void Delete(int id)
        {
            var item = _context.Set<UserAddress>().Find(id);
            if (item != null)
            {
                _context.Set<UserAddress>().Remove(item);
            }
        }

        /// <summary>
        /// Async method for fetching all user addresses from table.
        /// </summary>
        public async Task<IEnumerable<UserAddress>> GetAllAsync()
        {
            return await _context.Set<UserAddress>().ToListAsync();
        }

        /// <summary>
        /// Async method for fetching user address by id (primary key).
        /// </summary>
        public async Task<UserAddress> GetAsync(int id)
        {
            return await _context.Set<UserAddress>().FirstOrDefaultAsync(c => c.UserAddressId == id);
        }

        /// <summary>
        /// Async method for fetching all user addresses from table with pagination.
        /// </summary>
        public async Task<(IEnumerable<UserAddress> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<UserAddress>().OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<UserAddress>().CountAsync());
        }

        /// <summary>
        /// Async method for finding user addresses by expression with pagination.
        /// </summary>
        public async Task<(IEnumerable<UserAddress> Items, int TotalCount)> FindAsync(Expression<Func<UserAddress, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<UserAddress>().Where(expression);
            return (await query.OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
