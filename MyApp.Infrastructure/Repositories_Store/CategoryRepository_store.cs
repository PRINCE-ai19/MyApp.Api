using Dapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Common;
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

        public CategoryRepository_store(AppDbContext context)
        {
            _context = context;
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

        public async Task<SpResponse> AddAsync(Category category)
        {
            var connection = _context.Database.GetDbConnection();

            return await connection.QueryFirstOrDefaultAsync<SpResponse>(
                "sp_InsertCategory",
                new
                {
                    Name = category.Name,
                    Description = category.Description,
                    Code = category.Code,
                    RecordStatus = category.RecordStatus
                },
                commandType: CommandType.StoredProcedure
            ) ?? new SpResponse { Success = false, Message = "Lỗi không xác định từ Database." };
        }

        public async Task<SpResponse> UpdateAsync(Category category)
        {
            var connection = _context.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<SpResponse>(
                "sp_UpdateCategory",
                new
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Code = category.Code,
                    RecordStatus = category.RecordStatus
                },
                commandType: CommandType.StoredProcedure
            ) ?? new SpResponse { Success = false, Message = "Lỗi không xác định từ Database." };
        }

        public async Task<SpResponse> DeleteAsync(int id)
        {
            var connection = _context.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<SpResponse>(
                "sp_DeleteCategory",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            ) ?? new SpResponse { Success = false, Message = "Lỗi không xác định từ Database." };
        }
    }
}
