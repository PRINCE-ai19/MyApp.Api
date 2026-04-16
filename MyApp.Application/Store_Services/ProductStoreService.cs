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
    public class ProductStoreService : IProductStoreService
    {
        private readonly IProductRepository_store _repo;
        private readonly IMapper _mapper;
        public ProductStoreService(IProductRepository_store repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product_DTO>> GetAllProducts()
        {
            var entities = await _repo.GetAllProductAsync();
            return _mapper.Map<IEnumerable<Product_DTO>>(entities);
        }

        public async Task<SpResponse> Create(Product_DTO dto)
        {
            var newProduct = _mapper.Map<Product>(dto);
            return await _repo.AddAsync(newProduct);
        }

        public async Task<SpResponse> Update(int id, Product_DTO dto)
        {
            var existingProduct = await _repo.GetByIdAsync(id);

            _mapper.Map(dto, existingProduct);

            return await _repo.UpdateAsync(existingProduct);
        }

        public async Task<SpResponse> Delete(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<Product_DTO>> GetProductsByCategoryId(int categoryId)
        {
            var entities = await _repo.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<Product_DTO>>(entities);
        }
    }
}
