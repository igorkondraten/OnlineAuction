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
    /// Contains methods for processing entities in Categories table.
    /// </summary>
    public sealed class CategoriesRepository : ICategoriesRepository
    {
        private readonly IDataContext _context;

        public CategoriesRepository(IDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for fetching all categories from table.
        /// </summary>
        public IEnumerable<Category> GetAll()
        {
            return _context.Set<Category>().OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Method for fetching all categories from table with pagination.
        /// </summary>
        public (IEnumerable<Category> Items, int TotalCount) GetAll(int limit, int offset)
        {
            return (_context.Set<Category>().OrderBy(x => x.Name).Skip(offset).Take(limit).ToList(),
                _context.Set<Category>().Count());
        }

        /// <summary>
        /// Method for fetching category by id (primary key).
        /// </summary>
        public Category Get(int id)
        {
            return _context.Set<Category>().FirstOrDefault(c => c.CategoryId == id);
        }

        /// <summary>
        /// Method for finding categories by expression with pagination.
        /// </summary>
        public (IEnumerable<Category> Items, int TotalCount) Find(Expression<Func<Category, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<Category>().Where(expression);
            return (query.OrderBy(x => x.Name).Skip(offset).Take(limit).ToList(), query.Count());
        }

        /// <summary>
        /// Method for creating category.
        /// </summary>
        public void Create(Category item)
        {
            _context.Set<Category>().Add(item);
        }

        /// <summary>
        /// Method for updating category.
        /// </summary>
        public void Update(Category item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting category.
        /// </summary>
        public void Delete(int id)
        {
            var item = _context.Set<Category>().Find(id);
            if (item != null)
            {
                _context.Set<Category>().Remove(item);
            }
        }

        /// <summary>
        /// Async method for fetching all categories from table.
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Set<Category>().OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Async method for fetching category by id (primary key).
        /// </summary>
        public async Task<Category> GetAsync(int id)
        {
            return await _context.Set<Category>().FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        /// <summary>
        /// Async method for fetching all categories from table with pagination.
        /// </summary>
        public async Task<(IEnumerable<Category> Items, int TotalCount)> GetAllAsync(int limit, int offset)
        {
            return (await _context.Set<Category>().OrderBy(x => x.Name).Skip(offset).Take(limit).ToListAsync(),
                await _context.Set<Category>().CountAsync());
        }

        /// <summary>
        /// Async method for finding categories by expression with pagination.
        /// </summary>
        public async Task<(IEnumerable<Category> Items, int TotalCount)> FindAsync(Expression<Func<Category, bool>> expression, int limit, int offset)
        {
            var query = _context.Set<Category>().Where(expression);
            return (await query.OrderBy(x => x.Name).Skip(offset).Take(limit).ToListAsync(),
                await query.CountAsync());
        }
    }
}
