using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        (IEnumerable<T> Items, int TotalCount) GetAll(int limit, int offset);
        Task<(IEnumerable<T> Items, int TotalCount)> GetAllAsync(int limit, int offset);
        T Get(int id);
        Task<T> GetAsync(int id);
        (IEnumerable<T> Items, int TotalCount) Find(Expression<Func<T, bool>> predicate, int limit = 10, int offset = 0);
        Task<(IEnumerable<T> Items, int TotalCount)> FindAsync(Expression<Func<T, bool>> predicate, int limit = 10, int offset = 0);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
