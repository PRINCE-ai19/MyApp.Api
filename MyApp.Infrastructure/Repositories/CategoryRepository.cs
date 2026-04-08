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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {

            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> CheckCodeExisted(string code)
        {
            return await _context.Categories.AnyAsync(c => c.Code == code);
        }

        public async Task Add(MyApp.Domain.Entities.Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> CheckCodeExistedForOther(string code, int currentId)
        {
            return await _context.Categories.AnyAsync(c => c.Code == code && c.Id != currentId);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category); 
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetByIdAsyncRecordStatus(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

      
        public async Task<bool> HasRelatedProducts(int categoryId)
        {
           
            return false; 
        }
    }
}
