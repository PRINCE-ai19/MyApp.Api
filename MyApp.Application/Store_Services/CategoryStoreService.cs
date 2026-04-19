using AutoMapper;
using MyApp.Application.Model_DTO;
using MyApp.Application.Store_Interface;
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

        public async Task<IEnumerable<Category_DTO>> SearchCategoryByName(string? name)
        {
            var entities = await _repo.SearchCategory(name);
            return _mapper.Map<IEnumerable<Category_DTO>>(entities);
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
            var newCategory = _mapper.Map<Category>(dto);
            return await _repo.AddAsync(newCategory);
        }

        public async Task<SpResponse> Update(int id, Category_DTO dto)
        {
            var existingCategory = await _repo.GetByIdAsync(id);

            _mapper.Map(dto, existingCategory);

            return await _repo.UpdateAsync(existingCategory);
        }

        public async Task<SpResponse> Delete(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
