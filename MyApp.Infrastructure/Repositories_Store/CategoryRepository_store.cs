using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces_store;
using MyApp.Infrastructure.Data.Context;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repositories_Store
{
    public class CategoryRepository_store : ICategoryRepository_store 
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoryRepository_store(AppDbContext context , IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var connection = _context.Database.GetDbConnection();
            return await connection.QueryAsync<Category>(
                "sp_GetAllCategories",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var connection = _context.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<Category>(
                "sp_GetCategoryById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SpResponse> AddAsync(CategoryCreateParams category)
        {
            var connection = _context.Database.GetDbConnection();

            return await connection.QueryFirstOrDefaultAsync<SpResponse>(
                "sp_InsertCategory",
                 category,
                commandType: CommandType.StoredProcedure
            ) ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> UpdateAsync(CategoryUpdateParams parameters)
        {
            var connection = _context.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<SpResponse>(
                "sp_UpdateCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            ) ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> DeleteAsync(int id)
        {
            var connection = _context.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<SpResponse>(
                "sp_DeleteCategory",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            ) ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }
    }
}
