using AutoMapper;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Entities;
using MyApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Auto_mapper
{
    public class Products_mapper : Profile
    {
        public Products_mapper()
        {
        
            CreateMap<Product_DTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.RecordStatus, opt => opt.MapFrom(src => "1"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => 
                    string.IsNullOrWhiteSpace(src.CreatedAt) 
                        ? (DateTime?)null 
                        : DateTime.ParseExact(src.CreatedAt, "dd/MM/yyyy", new CultureInfo("vi-VN"))));

    
            CreateMap<Product, Product_DTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => 
                    src.CreatedAt.HasValue 
                        ? src.CreatedAt.Value.ToString("dd/MM/yyyy") 
                        : null));

            CreateMap<ProductUpdateDto, Product>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(d => d.RecordStatus , opt => opt.MapFrom(Src => "1"))
                .ReverseMap();

            CreateMap<Category_DTO, Category>()
                 .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.code))
                 .ForMember(dest => dest.RecordStatus, opt => opt.MapFrom(src => "1"))
                 .ForMember(
                    d => d.CreatedDate,
                    o => o.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.CreatedDate)
                            ? (DateTime?)null
                            : DateTime.ParseExact(src.CreatedDate, "dd/MM/yyyy", new CultureInfo("vi-VN"))
                    )
                  )
                .ReverseMap();
            CreateMap<Category, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
               

            CreateMap<Product, ProductDetailDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        

         

       
        }
    }
}
