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
    public sealed class UserAddressesRepository : IUserAddressesRepository
    {
        private readonly IDataContext _context;

        public UserAddressesRepository(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<UserAddress> GetAll()
        {
            return _context.Set<UserAddress>().ToList();
        }

        public (IEnumerable<UserAddress> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<UserAddress>().OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToList(),
                _context.Set<UserAddress>().Count());
        }

        public UserAddress Get(int id)
        {
            return _context.Set<UserAddress>().FirstOrDefault(c => c.UserAddressId == id);
        }

        public (IEnumerable<UserAddress> Items, int TotalCount) Find(Expression<Func<UserAddress, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<UserAddress>().Where(predicate);
            return (query.OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToList(), query.Count());
        }

        public void Create(UserAddress item)
        {
            _context.Set<UserAddress>().Add(item);
        }

        public void Update(UserAddress item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _context.Set<UserAddress>().Find(id);
            if (item != null)
            {
                _context.Set<UserAddress>().Remove(item);
            }
        }

        public async Task<IEnumerable<UserAddress>> GetAllAsync()
        {
            return await _context.Set<UserAddress>().ToListAsync();
        }

        public async Task<UserAddress> GetAsync(int id)
        {
            return await _context.Set<UserAddress>().FirstOrDefaultAsync(c => c.UserAddressId == id);
        }

        public async Task<(IEnumerable<UserAddress> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<UserAddress>().OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<UserAddress>().CountAsync());
        }

        public async Task<(IEnumerable<UserAddress> Items, int TotalCount)> FindAsync(Expression<Func<UserAddress, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<UserAddress>().Where(predicate);
            return (await query.OrderBy(x => x.UserAddressId).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
