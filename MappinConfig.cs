using AutoMapper;
using CursoWebAPI.Models;
using CursoWebAPI.Models.DTO;

namespace CursoWebAPI
{
    public class MappinConfig: Profile
    {

        public MappinConfig()
        {
            CreateMap<Villa,VillaDto>().ReverseMap();
            CreateMap<Villa,VillaCreateDto>().ReverseMap();
            CreateMap<Villa,VillaUpdateDto>().ReverseMap();
        }
    }
}
