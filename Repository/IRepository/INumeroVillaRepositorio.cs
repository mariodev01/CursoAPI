using CursoWebAPI.Models;

namespace CursoWebAPI.Repository.IRepository
{
    public interface INumeroVillaRepositorio: IRepository<NumeroVilla>
    {

        Task<NumeroVilla> Update(NumeroVilla entidad);
    }
}
