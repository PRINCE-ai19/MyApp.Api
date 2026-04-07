using MyApp.Application.Model_DTO;
using MyApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<dynamic>> GetProductsByCategoryId(int catId);
        Task<IEnumerable<dynamic>> GetAllProducts();

        Task AddProduct(Product_DTO dTO);

        Task UpdateProduct(ProductUpdateDto dto);
        
        Task<bool> DeleteProduct(int id);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<dynamic>> GetProductsByCategoryId(int catId)
        {
            var products = await _productRepo.GetByCategoryIdAsync(catId);


            return products.Select(p => new
            {

                p.Name,
                p.Price,
                p.Description

            });
        }

        public async Task<IEnumerable<dynamic>> GetAllProducts()
        {
            return await _productRepo.GetAllProductAsync();
        }

        public async Task AddProduct(Product_DTO dTO)
        {
            var newProduct = new MyApp.Domain.Entities.Product
            {
                Name = dTO.Name,
                Price = dTO.Price,
                Description = dTO.Description,
                CategoryId = dTO.CategoryId
            };
            await _productRepo.Add(newProduct);
        }

        public async Task UpdateProduct(ProductUpdateDto dto)
        {
            var existingProduct = await _productRepo.GetByIdProductAsync(dto.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = dto.Name;
                existingProduct.Price = dto.Price;
                existingProduct.Description = dto.Description;
                existingProduct.CategoryId = dto.CategoryId;
                await _productRepo.UpdateAsync(existingProduct);
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepo.GetByIdProductAsync(id);
            if (product == null)
            {
                return false; 
            }
            await _productRepo.DeleteAsync(product);
            return true;
        }
    }
   
}
