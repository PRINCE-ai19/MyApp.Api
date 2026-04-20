using AutoMapper;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Entities;
using BCrypt.Net;
using System.Globalization;

namespace MyApp.Application.Auto_mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserADD_DTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.PasswordHash)))
                .ForMember(dest => dest.RecordStatus, opt => opt.MapFrom(src => "1"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.CreatedAt)
                        ? (DateTime?)null
                        : DateTime.ParseExact(src.CreatedAt, "dd/MM/yyyy", new CultureInfo("vi-VN"))));

            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt ?? DateTime.Now))
                .ForMember(dest => dest.RecordStatus, opt => opt.MapFrom(src => "1"));

            CreateMap<User , User_DTO>().ReverseMap();

            CreateMap<Role , Role_DTO>()
                .ReverseMap();

            CreateMap<Permission, Permission_DTO>()
                .ReverseMap();

            CreateMap<Role_DTO , Role>()
                .ForMember(d => d.CreatedAt , op => op.MapFrom (src =>
                 string.IsNullOrWhiteSpace(src.CreatedAt)
                        ? (DateTime?)null
                        : DateTime.ParseExact(src.CreatedAt, "dd/MM/yyyy", new CultureInfo("vi-VN"))));
        }
    }
}
