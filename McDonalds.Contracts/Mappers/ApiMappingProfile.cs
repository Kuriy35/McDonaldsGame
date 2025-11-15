using AutoMapper;
using McDonalds.Models;
using McDonalds.ViewModels.Api;

namespace McDonalds.Mappings
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Resource, ResourceApiViewModel>().ReverseMap();
            CreateMap<DifficultyResource, DifficultyResourceApiViewModel>().ReverseMap();
        }
    }
}