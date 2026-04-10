using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category) // Eager loading để lấy thông tin Category
                .Where(p => p.CategoryId == categoryId ) // Lọc theo khóa ngoại và trạng thái
                .ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllProductAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> CheckCodeExisted(string code)
        {
            return await _context.Products.AnyAsync(p => p.Code == code);
        }

        public async Task AddAsync(Product product)
        {
           await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }


        public async Task<Product?> GetByIdProductAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id  );
        }

        public async Task<bool> CheckCodeExistedForOther(string code, int currentId)
        {
            return await _context.Products
                .AnyAsync(p => p.Code == code && p.Id != currentId );
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
