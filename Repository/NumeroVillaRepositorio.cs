using CursoWebAPI.Data;
using CursoWebAPI.Models;
using CursoWebAPI.Repository.IRepository;

namespace CursoWebAPI.Repository
{
    public class NumeroVillaRepositorio: Repository<NumeroVilla> , INumeroVillaRepositorio
    {
        private readonly ApplicationDbcontex _db;

        public NumeroVillaRepositorio(ApplicationDbcontex db) : base(db)
        {
            _db = db;
        }

        public async Task<NumeroVilla> Update(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroVillas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
