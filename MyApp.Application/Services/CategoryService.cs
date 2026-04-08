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
           
           var result = await _categoryRepo.GetAllCategoriesAsync();

            if(result == null || !result.Any())
            {
               return null;
            }

            return result;
        }

        public async Task AddCategory(Category_DTO dTO)
        {
            if (string.IsNullOrWhiteSpace(dTO.Name))
            {
                throw new Exception("Tên không được để trống babe ơi!");
            }
            if (dTO.Name.Length > 200)
            {
                throw new Exception("Tên dài quá rồi, dưới 200 ký tự thôi nè.");
            }
            if (string.IsNullOrWhiteSpace(dTO.code))
            {
                throw new Exception("Mã Code không được để trống.");
            }
            if (dTO.code.Length < 2 || dTO.code.Length > 20)
            {
                throw new Exception("Mã Code phải từ 2 đến 20 ký tự.");
            }
            
            bool isCodeExisted = await _categoryRepo.CheckCodeExisted(dTO.code);
            if(isCodeExisted)
            {
                throw new Exception("Mã Code đã tồn tại rồi, chọn mã khác đi.");
            }
            var NewCategory = new MyApp.Domain.Entities.Category
            {
                Name = dTO.Name,
                Description = dTO.Description,
                Code = dTO.code.Trim().ToUpper(),
                RecordStatus = "1"
            };
            await _categoryRepo.Add(NewCategory);
        }

        public async Task UpdateCategory(int id, CategoryUpdateDto dto)
        {

            if (id <= 0) throw new Exception("Id không hợp lệ babe ơi!");
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new Exception("Tên không được để trống!");
            if (string.IsNullOrWhiteSpace(dto.code)) throw new Exception("Mã Code không được để trống!");

            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null) {
            throw new Exception("Không tìm thấy danh mục với ID đã cho.");
            }

            bool isCodeUsedByOther = await _categoryRepo.CheckCodeExistedForOther(dto.code, id);
            if (isCodeUsedByOther)
            {
                throw new Exception("Mã Code này đã được một danh mục khác sử dụng rồi!");
            }

            if (existingCategory != null)
            {

                existingCategory.Name = dto.Name;
                existingCategory.Description = dto.Description;
                existingCategory.Code = dto.code.Trim().ToUpper();


                await _categoryRepo.UpdateAsync(existingCategory);
            }
        }

        public async Task DeleteCategory(int id)
        {

            if (id <= 0) throw new Exception("Id không hợp lệ để xóa babe ơi!");

            var existingCategory = await _categoryRepo.GetByIdAsyncRecordStatus(id);
            if (existingCategory == null)
            {
                throw new Exception("Danh mục này không tồn tại hoặc đã bị xóa trước đó rồi.");
            }

            bool hasProducts = await _categoryRepo.HasRelatedProducts(id);
            if (hasProducts)
            {
                throw new Exception("Danh mục này đang có sản phẩm, không xóa được đâu nè!");
            }

            existingCategory.RecordStatus = "0";

            await _categoryRepo.UpdateAsync(existingCategory);
        }
    }
}
