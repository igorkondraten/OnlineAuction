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
    /// Contains methods for processing entities in Bids table.
    /// </summary>
    public sealed class BidsRepository : IBidsRepository
    {
        private readonly IDataContext _context;

        public BidsRepository(IDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for fetching all bids from table.
        /// </summary>
        public IEnumerable<Bid> GetAll()
        {
            return _context.Set<Bid>().OrderBy(x => x.Price).ToList();
        }

        /// <summary>
        /// Method for fetching all bids from table with pagination.
        /// </summary>
        public (IEnumerable<Bid> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<Bid>().OrderBy(c => c.Price).Skip(offset).Take(limit).ToList(),
                _context.Set<Bid>().Count());
        }

        /// <summary>
        /// Method for fetching bid by id (primary key).
        /// </summary>
        public Bid Get(int id)
        {
            return _context.Set<Bid>().FirstOrDefault(c => c.BidId == id);
        }

        /// <summary>
        /// Method for finding bids by expression with pagination.
        /// </summary>
        public (IEnumerable<Bid> Items, int TotalCount) Find(Expression<Func<Bid, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<Bid>().Where(expression);
            return (query.OrderBy(x => x.Price).Skip(offset).Take(limit).ToList(), query.Count());
        }

        /// <summary>
        /// Method for creating bid.
        /// </summary>
        public void Create(Bid item)
        {
            _context.Set<Bid>().Add(item);
        }

        /// <summary>
        /// Method for updating bid.
        /// </summary>
        public void Update(Bid item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting bid.
        /// </summary>
        public void Delete(int id)
        {
            var item = _context.Set<Bid>().Find(id);
            if (item != null)
            {
                _context.Set<Bid>().Remove(item);
            }
        }

        /// <summary>
        /// Async method for fetching all bid from table.
        /// </summary>
        public async Task<IEnumerable<Bid>> GetAllAsync()
        {
            return await _context.Set<Bid>().OrderBy(x => x.Price).ToListAsync();
        }

        /// <summary>
        /// Async method for fetching bid by id (primary key).
        /// </summary>
        public async Task<Bid> GetAsync(int id)
        {
            return await _context.Set<Bid>().FirstOrDefaultAsync(c => c.BidId == id);
        }

        /// <summary>
        /// Async method for fetching all bids from table with pagination.
        /// </summary>
        public async Task<(IEnumerable<Bid> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<Bid>().OrderBy(c => c.Price).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<Bid>().CountAsync());
        }

        /// <summary>
        /// Async method for finding bids by expression with pagination.
        /// </summary>
        public async Task<(IEnumerable<Bid> Items, int TotalCount)> FindAsync(Expression<Func<Bid, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<Bid>().Where(expression);
            return (await query.OrderBy(x => x.Price).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }

        /// <summary>
        /// Method for fetching all bids by lot id.
        /// </summary>
        public async Task<IEnumerable<Bid>> GetAllByLotAsync(int lotId)
        {
            return await _context.Set<Bid>().Where(x => x.LotId == lotId).OrderBy(x => x.Price).ToListAsync();
        }

        /// <summary>
        /// Method for fetching all bids by user profile id.
        /// </summary>
        public async Task<IEnumerable<Bid>> GetAllByUserAsync(int userId)
        {
            return await _context.Set<Bid>().Where(x => x.PlacedUserId == userId).OrderBy(x => x.Date).ToListAsync();
        }
    }
}
