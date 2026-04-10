using AutoMapper;
using Microsoft.Extensions.Localization;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Domain.Entities;
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
        
        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoryService(ICategoryRepository categoryRepo , IMapper mapper , IStringLocalizer<SharedResource> localizer)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<dynamic>> GetListCategoryForUI()
        {
           
           var result = await _categoryRepo.GetAllCategoriesAsync();

          return _mapper.Map<IEnumerable<Category_DTO>>(result);


        }

        public async Task AddCategory(Category_DTO dTO)
        {
          
            bool isCodeExisted = await _categoryRepo.CheckCodeExisted(dTO.code);
            if(isCodeExisted)
            {
                throw new Exception(_localizer["DuplicateProductCode"]);
            }
          /*  var NewCategory = new MyApp.Domain.Entities.Category
            {
                Name = dTO.Name,
                Description = dTO.Description,
                Code = dTO.code.Trim().ToUpper(),
                RecordStatus = "1"
            };*/
          var NewCategory = _mapper.Map<Category>(dTO);
            NewCategory.Code = dTO.code.Trim().ToUpper();
            NewCategory.RecordStatus="1";
            await _categoryRepo.Add(NewCategory);
        }

        public async Task UpdateCategory(int id, CategoryUpdateDto dto)
        {

            if (id <= 0) throw new Exception(_localizer["InvalidProductId"]);
         

            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null) {
            throw new Exception(_localizer["InvalidProductId"]);
            }

            bool isCodeUsedByOther = await _categoryRepo.CheckCodeExistedForOther(dto.code, id);
            if (isCodeUsedByOther)
            {
                throw new Exception(_localizer["DuplicateProductCode"]);
            }

            if (existingCategory != null)
            {

                var updatedCategory = _mapper.Map(dto, existingCategory);
                updatedCategory.Code = dto.code.Trim().ToUpper();
                await _categoryRepo.UpdateAsync(updatedCategory);
            }
        }

        public async Task DeleteCategory(int id)
        {

            if (id <= 0) throw new Exception(_localizer["InvalidProductId"]);

            var existingCategory = await _categoryRepo.GetByIdAsyncRecordStatus(id);
            if (existingCategory == null)
            {
                throw new Exception(_localizer["ProductAlreadyDeleted"]);
            }

            bool hasProducts = await _categoryRepo.HasRelatedProducts(id);
            if (hasProducts)
            {
                throw new Exception(_localizer["CategoryHasProducts"]);
            }
            if(existingCategory.RecordStatus == "0")
            {
                throw new Exception(_localizer["ProductAlreadyDeleted"]);
            }
            existingCategory.RecordStatus = "0";

            await _categoryRepo.UpdateAsync(existingCategory);
        }
    }
}
