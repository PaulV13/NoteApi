using System.Linq.Expressions;

namespace NoteApi.Data.Repositories
{
    public interface IRepositoryGeneric<T> where T : class
    {

        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Expression<Func<T, bool>> expression);
        Task<T> GetById(int id);
        Task<T> Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
