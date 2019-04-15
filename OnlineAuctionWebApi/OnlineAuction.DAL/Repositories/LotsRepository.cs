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
    /// Contains methods for processing entities in Lots table.
    /// </summary>
    public sealed class LotsRepository : ILotsRepository
    {
        private readonly IDataContext _context;

        public LotsRepository(IDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for fetching all lots from table.
        /// </summary>
        public IEnumerable<Lot> GetAll()
        {
            return _context.Set<Lot>().ToList();
        }

        /// <summary>
        /// Method for fetching all lots from table with pagination.
        /// </summary>
        public (IEnumerable<Lot> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<Lot>().OrderBy(x => x.LotId).Skip(offset).Take(limit).ToList(),
                _context.Set<Lot>().Count());
        }

        /// <summary>
        /// Method for fetching lot by id (primary key).
        /// </summary>
        public Lot Get(int id)
        {
            return _context.Set<Lot>().FirstOrDefault(c => c.LotId == id);
        }

        /// <summary>
        /// Method for finding lot by expression with pagination.
        /// </summary>
        public (IEnumerable<Lot> Items, int TotalCount) Find(Expression<Func<Lot, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<Lot>().Where(expression);
            return (query.OrderBy(x => x.LotId).Skip(offset).Take(limit).ToList(), query.Count());
        }

        /// <summary>
        /// Method for creating lot.
        /// </summary>
        public void Create(Lot item)
        {
            _context.Set<Lot>().Add(item);
        }

        /// <summary>
        /// Method for updating lot.
        /// </summary>
        public void Update(Lot item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting lot.
        /// </summary>
        public void Delete(int id)
        {
            var item = _context.Set<Lot>().Find(id);
            if (item != null)
            {
                _context.Set<Lot>().Remove(item);
            }
        }

        /// <summary>
        /// Async method for fetching all lots from table.
        /// </summary>
        public async Task<IEnumerable<Lot>> GetAllAsync()
        {
            return await _context.Set<Lot>().ToListAsync();
        }

        /// <summary>
        /// Async method for fetching lots by id (primary key).
        /// </summary>
        public async Task<Lot> GetAsync(int id)
        {
            return await _context.Set<Lot>().FirstOrDefaultAsync(c => c.LotId == id);
        }

        /// <summary>
        /// Async method for fetching all lots from table with pagination.
        /// </summary>
        public async Task<(IEnumerable<Lot> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<Lot>().OrderBy(x => x.LotId).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<Lot>().CountAsync());
        }

        /// <summary>
        /// Async method for finding lots by expression with pagination.
        /// </summary>
        public async Task<(IEnumerable<Lot> Items, int TotalCount)> FindAsync(Expression<Func<Lot, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<Lot>().Where(expression);
            return (await query.OrderBy(x => x.LotId).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
