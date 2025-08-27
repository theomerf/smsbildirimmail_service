using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> FindAll(bool trackChanges);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        void Create(T entity);

        void Remove(T entity);

        Task<int> CountAsync(bool trackChanges);
        void Attach(T entity);
        DbEntityEntry<T> Entry(T entity);
    }
}
