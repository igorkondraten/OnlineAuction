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
    public sealed class UserProfilesRepository : IUserProfilesRepository
    {
        private readonly IDataContext _context;

        public UserProfilesRepository(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _context.Set<UserProfile>().ToList();
        }

        public (IEnumerable<UserProfile> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<UserProfile>().OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToList(), _context.Set<UserProfile>().Count());
        }

        public UserProfile Get(int id)
        {
            return _context.Set<UserProfile>().FirstOrDefault(c => c.UserProfileId == id);
        }

        public (IEnumerable<UserProfile> Items, int TotalCount) Find(Expression<Func<UserProfile, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<UserProfile>().Where(predicate);
            return (query.ToList().OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToList(), query.Count());
        }

        public void Create(UserProfile item)
        {
            _context.Set<UserProfile>().Add(item);
        }

        public void Update(UserProfile item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _context.Set<UserProfile>().Find(id);
            if (item != null)
            {
                _context.Set<UserProfile>().Remove(item);
            }
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await _context.Set<UserProfile>().ToListAsync();
        }

        public async Task<UserProfile> GetAsync(int id)
        {
            return await _context.Set<UserProfile>().FirstOrDefaultAsync(c => c.UserProfileId == id);
        }

        public async Task<(IEnumerable<UserProfile> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<UserProfile>().OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<UserProfile>().CountAsync());
        }

        public async Task<(IEnumerable<UserProfile> Items, int TotalCount)> FindAsync(Expression<Func<UserProfile, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<UserProfile>().Where(predicate);
            return (await query.OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
