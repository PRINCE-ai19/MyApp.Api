using MyApp.Application.Model_DTO;
using MyApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<dynamic>> GetListCategoryForUI()
        {
           
            return await _categoryRepo.GetAllCategoriesAsync();
        }

        public async Task AddCategory(Category_DTO dTO)
        {
           var NewCategory = new MyApp.Domain.Entities.Category
            {
                Name = dTO.Name,
                Description = dTO.Description
            };
            await _categoryRepo.Add(NewCategory);
        }

        public async Task UpdateCategory(CategoryUpdateDto dto)
        {
            
            var existingCategory = await _categoryRepo.GetByIdAsync(dto.Id);

            if (existingCategory != null)
            {
               
                existingCategory.Name = dto.Name;
                existingCategory.Description = dto.Description;

              
                await _categoryRepo.UpdateAsync(existingCategory);
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
           
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                return false; 
            }

            //  Gọi Repo để thực hiện xóa
            await _categoryRepo.DeleteAsync(category);
            return true;
        }
    }
}
