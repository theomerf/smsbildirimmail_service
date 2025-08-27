using Repositories.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : class, new()
    {
        protected readonly RepositoryContext _context;

        protected RepositoryBase(RepositoryContext context) 
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);  
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges
            ? _context.Set<T>()
            : _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges
                ? _context.Set<T>().Where(expression)
                : _context.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task<int> CountAsync(bool trackChanges)
        {
            return trackChanges
                ? await _context.Set<T>().CountAsync()
                : await _context.Set<T>().AsNoTracking().CountAsync();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }

        public DbEntityEntry<T> Entry(T entity) 
        {
            return _context.Entry(entity);
        }
    }
}
