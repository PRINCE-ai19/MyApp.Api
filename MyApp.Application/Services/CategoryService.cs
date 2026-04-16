using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        
        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResource> _localizer;

        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepo , IMapper mapper , IStringLocalizer<SharedResource> localizer , ILogger<CategoryService> logger)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<IEnumerable<dynamic>> GetListCategoryForUI()
        {
           
           var result = await _categoryRepo.GetAllCategoriesAsync();

          return _mapper.Map<IEnumerable<Category_DTO>>(result);


        }

        public async Task<SpResponse> AddCategory(Category_DTO dTO)
        {
          
            bool isCodeExisted = await _categoryRepo.CheckCodeExisted(dTO.code);
            if(isCodeExisted)
            {
               _logger.LogError(_localizer["DuplicateProductCode"]);
               return new SpResponse { Success = false, Message = _localizer["DuplicateProductCode"] };
            }

            var NewCategory = _mapper.Map<Category>(dTO);
            NewCategory.Code = dTO.code.Trim().ToUpper();
            NewCategory.RecordStatus="1";
            await _categoryRepo.Add(NewCategory);
            return new SpResponse { Success = true, Message = _localizer["InsertCategorySuccess"] };
        }

        public async Task<SpResponse> UpdateCategory(int id, CategoryUpdateDto dto)
        {

            if (id <= 0)
            {
                _logger.LogError(_localizer["InvalidProductId"]);
                return new SpResponse { Success = false, Message = _localizer["InvalidProductId"] };
            }

            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null) {
                _logger.LogError(_localizer["InvalidProductId"]);
                return new SpResponse { Success = false, Message = _localizer["InvalidProductId"] };
            }

            bool isCodeUsedByOther = await _categoryRepo.CheckCodeExistedForOther(dto.code, id);
            if (isCodeUsedByOther)
            {
                _logger.LogError(_localizer["DuplicateProductCode"]);
                return new SpResponse { Success = false, Message = _localizer["DuplicateProductCode"] };
            }

            var updatedCategory = _mapper.Map(dto, existingCategory);
            updatedCategory.Code = dto.code.Trim().ToUpper();
            await _categoryRepo.UpdateAsync(updatedCategory);
            return new SpResponse { Success = true, Message = _localizer["UpdateCategorySuccess"] };
        }

        public async Task<SpResponse> DeleteCategory(int id)
        {

            if (id <= 0)
            {
                _logger.LogError(_localizer["InvalidProductId"]);
                return new SpResponse { Success = false, Message = _localizer["InvalidProductId"] };
            }

            var existingCategory = await _categoryRepo.GetByIdAsyncRecordStatus(id);
            if (existingCategory == null)
            {
                _logger.LogError(_localizer["ProductAlreadyDeleted"]);
                return new SpResponse { Success = false, Message = _localizer["ProductAlreadyDeleted"] };
            }

            bool hasProducts = await _categoryRepo.HasRelatedProducts(id);
            if (hasProducts)
            {
               _logger.LogError(_localizer["CategoryHasProducts"]);
               return new SpResponse { Success = false, Message = _localizer["CategoryHasProducts"] };
            }

            if(existingCategory.RecordStatus == "0")
            {
               _logger.LogError (_localizer["ProductAlreadyDeleted"]);
               return new SpResponse { Success = false, Message = _localizer["ProductAlreadyDeleted"] };
            }

            existingCategory.RecordStatus = "0";

            await _categoryRepo.UpdateAsync(existingCategory);

            return new SpResponse { Success = true, Message = _localizer["DeleteCategorySuccess"] };
        }
    }
}
