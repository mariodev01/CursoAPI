using System.Linq.Expressions;

namespace CursoWebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        Task Add(T entity);

        Task<List<T>> GetAll(Expression<Func<T,bool>>? filtro = null);

        Task<T> Get(Expression<Func<T, bool>> filtro = null, bool tracked = true);

        Task Grabar();

        Task Delete(T entity);
    }
}
