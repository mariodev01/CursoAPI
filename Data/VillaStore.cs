using CursoWebAPI.Models.DTO;

namespace CursoWebAPI.Data
{
    public static class VillaStore
    {

        public static List<VillaDto> villaDtos = new List<VillaDto>
        {
            new VillaDto{Id = 1,Nombre = "Villa Campo Rico",Ocupantes=5,MetrosCuadrados=88},
            new VillaDto{Id = 2,Nombre = "Villa Verde",Ocupantes=4,MetrosCuadrados=60}
        };
    }
}
