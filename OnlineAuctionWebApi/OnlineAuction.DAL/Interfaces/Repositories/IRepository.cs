using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Interfaces.Repositories
{
    /// <summary>
    /// Interface for DB repositories.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Method for fetching all data from table.
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Async method for fetching all data from table.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Method for fetching all data from table with pagination.
        /// </summary>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>Collection of items and total count of all items.</returns>
        (IEnumerable<T> Items, int TotalCount) GetAll(int limit, int offset);

        /// <summary>
        /// Async method for fetching all data from table with pagination.
        /// </summary>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>Collection of items and total count of all items.</returns>
        Task<(IEnumerable<T> Items, int TotalCount)> GetAllAsync(int limit, int offset);

        /// <summary>
        /// Method for fetching entity by id (primary key).
        /// </summary>
        T Get(int id);

        /// <summary>
        /// Async method for fetching entity by id (primary key).
        /// </summary>
        Task<T> GetAsync(int id);

        /// <summary>
        /// Method for finding entities by expression with pagination.
        /// </summary>
        /// <param name="expression">Expression for finding entities.</param>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>Collection of items and total count of all items.</returns>
        (IEnumerable<T> Items, int TotalCount) Find(Expression<Func<T, bool>> expression, int limit = 10, int offset = 0);

        /// <summary>
        /// Async method for finding entities by expression with pagination.
        /// </summary>
        /// <param name="expression">Expression for finding entities.</param>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>Collection of items and total count of all items.</returns>
        Task<(IEnumerable<T> Items, int TotalCount)> FindAsync(Expression<Func<T, bool>> expression, int limit = 10, int offset = 0);

        /// <summary>
        /// Method for creating entity.
        /// </summary>
        void Create(T item);

        /// <summary>
        /// Method for updating entity.
        /// </summary>
        void Update(T item);

        /// <summary>
        /// Method for deleting entity.
        /// </summary>
        void Delete(int id);
    }
}