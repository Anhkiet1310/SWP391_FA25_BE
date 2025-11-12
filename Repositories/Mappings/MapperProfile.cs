using AutoMapper;
using Repositories.DTOs.Contract;
using Repositories.DTOs.Payment;
using Repositories.DTOs.Schedule;
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

            //Contract mappings
            CreateMap<Contract, ContractDto>();
            CreateMap<ContractCreateDto, Contract>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<ContractUpdateDto, Contract>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            //Schedule mappings
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<ScheduleCreateDto, Schedule>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<ScheduleUpdateDto, Schedule>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            //CreateMap<Payment, PaymentListItemDto>()
            //.ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.CarUser.Car.CarName))
            //.ForMember(dest => dest.PlateNumber, opt => opt.MapFrom(src => src.CarUser.Car.PlateNumber))
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}