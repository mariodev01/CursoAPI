using CursoWebAPI.Data;
using CursoWebAPI.Models;
using CursoWebAPI.Repository.IRepository;

namespace CursoWebAPI.Repository
{
    public class VillaRepositorio: Repository<Villa> , IVillaRepositorio
    {
        private readonly ApplicationDbcontex _db;

        public VillaRepositorio(ApplicationDbcontex db) : base(db)
        {
            _db = db;
        }

        public async Task<Villa> Update(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.Villas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
