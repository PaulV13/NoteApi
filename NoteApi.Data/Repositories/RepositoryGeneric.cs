using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NoteApi.Data.Repositories
{
    public abstract class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : class
    {
        protected readonly ApplicationDBContext _context;

        protected RepositoryGeneric(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AsNoTracking().Where(expression).FirstOrDefaultAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
