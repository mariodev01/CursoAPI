using CursoWebAPI.Data;
using CursoWebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CursoWebAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbcontex _db;
        internal DbSet<T> Set;
        public Repository(ApplicationDbcontex db)
        {
            _db = db;
            this.Set = db.Set<T>();
        }

        public async Task Add(T entity)
        {
            await Set.AddAsync(entity);
            await Grabar();
        }

        public async Task Delete(T entity)
        {
            Set.Remove(entity);
            await Grabar();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filtro = null,bool tracked = true)
        {
            IQueryable<T> query = Set;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if(filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = Set;

            if(filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.ToListAsync();
        }
        public async Task Grabar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
