using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Services;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data.Context;
using MyApp.Infrastructure.Repositories;
using MyApp.Application.Resources;
using MyApp.Api.Filters;
using MyApp.Application.Auto_mapper;
using MyApp.Domain.Interfaces_store;
using MyApp.Infrastructure.Repositories_Store;
using MyApp.Application.Store_Services;

namespace MyApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryRepository_store, CategoryRepository_store>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryStoreService, CategoryStoreService>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MyApp.Application.AssemblyReference).Assembly));
            builder.Services.AddAutoMapper(typeof(Products_mapper));

            builder.Services.AddLocalization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            builder.Services.AddControllers()
                .AddDataAnnotationsLocalization(op =>
                {
                    op.DataAnnotationLocalizerProvider = (type, factory) =>
                    
                      factory.Create(typeof(SharedResource));
                    
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerCultureFilter>();
            });

            var app = builder.Build();

            var supportedCultures = new[] { "en-US", "vi-VN" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("vi-VN") // Đặt ngôn ngữ mặc định là tiếng Việt
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            // cấu hình middlware dể cho nó nhận diện cái query string ?culture=vi-VN hoặc ?culture=en-US để thay đổi ngôn ngữ
            localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

         

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
