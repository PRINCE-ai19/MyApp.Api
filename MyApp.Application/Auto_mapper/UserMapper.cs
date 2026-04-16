using AutoMapper;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Entities;
using BCrypt.Net;

namespace MyApp.Application.Auto_mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHass, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.RecordStatus, opt => opt.MapFrom(src => "1"));
        }
    }
}
