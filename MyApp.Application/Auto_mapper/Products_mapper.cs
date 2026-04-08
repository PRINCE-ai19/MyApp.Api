using AutoMapper;
using Microsoft.VisualBasic;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Entities;
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
                .ForMember(dest => dest.RecordStatus, opt => opt.MapFrom(src => "1"))
                .ForMember(
                    d => d.CreatedAt,
                    o => o.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.CreatedAt)
                            ? (DateTime?)null
                            : DateTime.ParseExact(src.CreatedAt, "dd/MM/yyyy", new CultureInfo("vi-VN"))
                    )
                )
                .ReverseMap()

                .ForMember(
                    d => d.CreatedAt,
                    o => o.MapFrom(src =>
                        src.CreatedAt.HasValue
                            ? src.CreatedAt.Value.ToString("dd/MM/yyyy")
                            : null
                    )
                );

            CreateMap<ProductUpdateDto, Product>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(d => d.RecordStatus , opt => opt.MapFrom(Src => "1"))
                .ReverseMap();

            CreateMap<Category_DTO, Category>().ReverseMap();
               

            CreateMap<Product, ProductDetailDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
       
        }
    }
}
