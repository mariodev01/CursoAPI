using CursoWebAPI.Models;

namespace CursoWebAPI.Repository.IRepository
{
    public interface IVillaRepositorio: IRepository<Villa>
    {

        Task<Villa> Update(Villa entidad);
    }
}
