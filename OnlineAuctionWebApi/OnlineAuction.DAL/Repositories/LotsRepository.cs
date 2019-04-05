using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;
using OnlineAuction.DAL.Interfaces.Repositories;

namespace OnlineAuction.DAL.Repositories
{
    public sealed class LotsRepository : ILotsRepository
    {
        private readonly IDataContext _context;

        public LotsRepository(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Lot> GetAll()
        {
            return _context.Set<Lot>().ToList();
        }

        public (IEnumerable<Lot> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<Lot>().OrderBy(x => x.LotId).Skip(offset).Take(limit).ToList(),
                _context.Set<Lot>().Count());
        }

        public Lot Get(int id)
        {
            return _context.Set<Lot>().FirstOrDefault(c => c.LotId == id);
        }

        public (IEnumerable<Lot> Items, int TotalCount) Find(Expression<Func<Lot, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<Lot>().Where(predicate);
            return (query.OrderBy(x => x.LotId).Skip(offset).Take(limit).ToList(), query.Count());
        }

        public void Create(Lot item)
        {
            _context.Set<Lot>().Add(item);
        }

        public void Update(Lot item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _context.Set<Lot>().Find(id);
            if (item != null)
            {
                _context.Set<Lot>().Remove(item);
            }
        }

        public async Task<IEnumerable<Lot>> GetAllAsync()
        {
            return await _context.Set<Lot>().ToListAsync();
        }

        public async Task<Lot> GetAsync(int id)
        {
            return await _context.Set<Lot>().FirstOrDefaultAsync(c => c.LotId == id);
        }

        public async Task<(IEnumerable<Lot> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<Lot>().OrderBy(x => x.LotId).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<Lot>().CountAsync());
        }

        public async Task<(IEnumerable<Lot> Items, int TotalCount)> FindAsync(Expression<Func<Lot, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<Lot>().Where(predicate);
            return (await query.OrderBy(x => x.LotId).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
