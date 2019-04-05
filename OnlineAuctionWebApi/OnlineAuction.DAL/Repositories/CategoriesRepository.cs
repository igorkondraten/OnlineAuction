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
    public sealed class CategoriesRepository : ICategoriesRepository
    {
        private readonly IDataContext _context;

        public CategoriesRepository(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Set<Category>().OrderBy(x => x.Name).ToList();
        }

        public (IEnumerable<Category> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<Category>().OrderBy(x => x.Name).Skip(offset).Take(limit).ToList(),
                _context.Set<Category>().Count());
        }

        public Category Get(int id)
        {
            return _context.Set<Category>().FirstOrDefault(c => c.CategoryId == id);
        }

        public (IEnumerable<Category> Items, int TotalCount) Find(Expression<Func<Category, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<Category>().Where(predicate);
            return (query.OrderBy(x => x.Name).Skip(offset).Take(limit).ToList(), query.Count());
        }

        public void Create(Category item)
        {
            _context.Set<Category>().Add(item);
        }

        public void Update(Category item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _context.Set<Category>().Find(id);
            if (item != null)
            {
                _context.Set<Category>().Remove(item);
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Set<Category>().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Category> GetAsync(int id)
        {
            return await _context.Set<Category>().FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<(IEnumerable<Category> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<Category>().OrderBy(x => x.Name).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<Category>().CountAsync());
        }

        public async Task<(IEnumerable<Category> Items, int TotalCount)> FindAsync(Expression<Func<Category, bool>> predicate, int limit, int offset)
        {
            var query = _context.Set<Category>().Where(predicate);
            return (await query.OrderBy(x => x.Name).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
