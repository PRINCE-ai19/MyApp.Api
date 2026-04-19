using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApp.Api.Filters;
using MyApp.Application.Auto_mapper;
using MyApp.Application.Resources;
using MyApp.Application.Services;
using MyApp.Application.Store_Services;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Interfaces_store;
using MyApp.Infrastructure.Data.Context;
using MyApp.Infrastructure.Repositories;
using MyApp.Infrastructure.Repositories_Store;
//using MyApp.Infrastructure.Services;
using MyApp.Infrastructure.Helpers;
using System.Text;
using Serilog;
using MyApp.Application.Store_Interface;

namespace MyApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryRepository_store, CategoryRepository_store>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryStoreService, CategoryStoreService>();
            // builder.Services.AddScoped<IAuthService, AuthService>();
            //  builder.Services.AddScoped<IUserRepository, UserRepository>();
            // builder.Services.AddScoped<IJwtRepository, JwtRepository>();
            builder.Services.AddScoped<IUserStoreService, UserStoreService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IRoleSoteService, RoleStoreService>();
             builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<IPermissionStoreService, PermissionStoreService>();


            builder.Services.AddScoped<IProductRepository_store, ProductRepository_store>();
            builder.Services.AddScoped<IProductStoreService, ProductStoreService>();
            builder.Services.AddScoped<IStoreHelper, StoreHelper>();

          
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MyApp.Application.AssemblyReference).Assembly));
            builder.Services.AddAutoMapper(typeof(Products_mapper));

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // mỗi lần gửi request thằng bóc token đọc thời gian hết hạn 
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
                    };
                });

           
            Serilog.Debugging.SelfLog.Enable(msg => 
            {
                Console.WriteLine(msg);
                File.AppendAllText("Logs/selflog.txt", msg + Environment.NewLine);
            });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog(); // sử dụng Serilog 

       

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
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Nhập token của bạn theo định dạng: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                   {
                     new OpenApiSecurityScheme
                      {
                       Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                      },
                     new string[]{}
                   }
                });
            });
             
                Log.Information("Ứng dụng đang được khởi động");
            var app = builder.Build();
                var supportedCultures = new[] { "en-US", "vi-VN" };
                var localizationOptions = new RequestLocalizationOptions()
                    .SetDefaultCulture("vi-VN") 
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

                app.UseSerilogRequestLogging(); // Middleware để ghi log cho mỗi request

                app.UseHttpsRedirection();

                app.UseAuthentication();

                app.UseAuthorization();


                app.MapControllers();


            

            app.Run();
        }
    }
}
