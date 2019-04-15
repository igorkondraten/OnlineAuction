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
    /// Contains methods for processing entities in UserProfiles table.
    /// </summary>
    public sealed class UserProfilesRepository : IUserProfilesRepository
    {
        private readonly IDataContext _context;

        public UserProfilesRepository(IDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for fetching all user profiles from table.
        /// </summary>
        public IEnumerable<UserProfile> GetAll()
        {
            return _context.Set<UserProfile>().ToList();
        }

        /// <summary>
        /// Method for fetching all user profiles from table with pagination.
        /// </summary>
        public (IEnumerable<UserProfile> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<UserProfile>().OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToList(), _context.Set<UserProfile>().Count());
        }

        /// <summary>
        /// Method for fetching user profile by id (primary key).
        /// </summary>
        public UserProfile Get(int id)
        {
            return _context.Set<UserProfile>().FirstOrDefault(c => c.UserProfileId == id);
        }

        /// <summary>
        /// Method for finding user profiles by expression with pagination.
        /// </summary>
        public (IEnumerable<UserProfile> Items, int TotalCount) Find(Expression<Func<UserProfile, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<UserProfile>().Where(expression);
            return (query.ToList().OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToList(), query.Count());
        }

        /// <summary>
        /// Method for creating user profile.
        /// </summary>
        public void Create(UserProfile item)
        {
            _context.Set<UserProfile>().Add(item);
        }

        /// <summary>
        /// Method for updating user profile.
        /// </summary>
        public void Update(UserProfile item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting user profile.
        /// </summary>
        public void Delete(int id)
        {
            var item = _context.Set<UserProfile>().Find(id);
            if (item != null)
            {
                _context.Set<UserProfile>().Remove(item);
            }
        }

        /// <summary>
        /// Async method for fetching all user profiles from table.
        /// </summary>
        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await _context.Set<UserProfile>().ToListAsync();
        }

        /// <summary>
        /// Async method for fetching user profile by id (primary key).
        /// </summary>
        public async Task<UserProfile> GetAsync(int id)
        {
            return await _context.Set<UserProfile>().FirstOrDefaultAsync(c => c.UserProfileId == id);
        }

        /// <summary>
        /// Async method for fetching all user profiles from table with pagination.
        /// </summary>
        public async Task<(IEnumerable<UserProfile> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<UserProfile>().OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<UserProfile>().CountAsync());
        }

        /// <summary>
        /// Async method for finding user profiles by expression with pagination.
        /// </summary>
        public async Task<(IEnumerable<UserProfile> Items, int TotalCount)> FindAsync(Expression<Func<UserProfile, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<UserProfile>().Where(expression);
            return (await query.OrderBy(x => x.UserProfileId).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
