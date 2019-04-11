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
    public sealed class BidsRepository : IBidsRepository
    {
        private readonly IDataContext _context;

        public BidsRepository(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Bid> GetAll()
        {
            return _context.Set<Bid>().OrderBy(x => x.Price).ToList();
        }

        public (IEnumerable<Bid> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<Bid>().OrderBy(c => c.Price).Skip(offset).Take(limit).ToList(),
                _context.Set<Bid>().Count());
        }

        public Bid Get(int id)
        {
            return _context.Set<Bid>().FirstOrDefault(c => c.BidId == id);
        }

        public (IEnumerable<Bid> Items, int TotalCount) Find(Expression<Func<Bid, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<Bid>().Where(predicate);
            return (query.OrderBy(x => x.Price).Skip(offset).Take(limit).ToList(), query.Count());
        }

        public void Create(Bid item)
        {
            _context.Set<Bid>().Add(item);
        }

        public void Update(Bid item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _context.Set<Bid>().Find(id);
            if (item != null)
            {
                _context.Set<Bid>().Remove(item);
            }
        }

        public async Task<IEnumerable<Bid>> GetAllAsync()
        {
            return await _context.Set<Bid>().OrderBy(x => x.Price).ToListAsync();
        }

        public async Task<Bid> GetAsync(int id)
        {
            return await _context.Set<Bid>().FirstOrDefaultAsync(c => c.BidId == id);
        }

        public async Task<(IEnumerable<Bid> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<Bid>().OrderBy(c => c.Price).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<Bid>().CountAsync());
        }

        public async Task<(IEnumerable<Bid> Items, int TotalCount)> FindAsync(Expression<Func<Bid, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<Bid>().Where(predicate);
            return (await query.OrderBy(x => x.Price).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }

        public async Task<IEnumerable<Bid>> GetAllByLotAsync(int lotId)
        {
            return await _context.Set<Bid>().Where(x => x.LotId == lotId).OrderBy(x => x.Price).ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetAllByUserAsync(int userId)
        {
            return await _context.Set<Bid>().Where(x => x.PlacedUserId == userId).OrderBy(x => x.Date).ToListAsync();
        }
    }
}
