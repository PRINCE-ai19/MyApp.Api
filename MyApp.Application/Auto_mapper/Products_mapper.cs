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
                // If DTO.CreatedAt is null or empty, set null (if Product.CreatedAt is nullable) or DateTime.MinValue otherwise.
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
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();
        }
    }
}
