using AutoMapper;
using Repositories.DTOs.User;
using Repositories.Entities;

namespace Repositories.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<User, UserUpdateProfileDto>().ReverseMap();
        }
    }
}
