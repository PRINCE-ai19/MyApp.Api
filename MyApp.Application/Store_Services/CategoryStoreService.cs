using AutoMapper;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces_store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Services
{
    public class CategoryStoreService : ICategoryStoreService
    {
        private readonly ICategoryRepository_store _repo;
        private readonly IMapper _mapper;

        public CategoryStoreService(ICategoryRepository_store repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category_DTO>> GetAllCategories()
        {
            var entities = await _repo.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<Category_DTO>>(entities);
        }

        public async Task<Category_DTO?> GetById(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity != null ? _mapper.Map<Category_DTO>(entity) : null;
        }

        public async Task<SpResponse> Create(Category_DTO dto)
        {
            var newCategory = _mapper.Map<CategoryCreateParams>(dto);
            return await _repo.AddAsync(newCategory);
        }

        public async Task<SpResponse> Update(int id, Category_DTO dto)
        {
            var existingCategory = await _repo.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return new SpResponse { Success = false, Message = "Không tìm thấy danh mục." };
            }

            _mapper.Map(dto, existingCategory);

            var updateParams = _mapper.Map<CategoryUpdateParams>(existingCategory);

            return await _repo.UpdateAsync(updateParams);
        }

        public async Task<SpResponse> Delete(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
