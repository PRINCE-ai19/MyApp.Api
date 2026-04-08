using AutoMapper;
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
        Task<dynamic?> GetProductById(int id);

        Task AddProduct(Product_DTO dTO);

        Task UpdateProduct(int id, ProductUpdateDto dto);

        Task DeleteProduct(int id);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo , ICategoryRepository categoryRepo , IMapper mapper)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<dynamic>> GetProductsByCategoryId(int catId)
        {
            var products = await _productRepo.GetByCategoryIdAsync(catId);


            return products.Select(p => new
            {
                Id = p.Id,
                p.Name,
                p.Price,
                p.Description,
                p.Img
            });
        }

        public async Task<IEnumerable<dynamic>> GetAllProducts()
        {
            return await _productRepo.GetAllProductAsync();
        }

        public async Task<dynamic?> GetProductById(int id)
        {
            var p = await _productRepo.GetByIdProductAsync(id);
            if (p == null) return null;

            return new
            {
                Id = p.Id,
                p.Name,
                p.Price,
                p.Description,
                p.Img,
                p.StockQuantity,
                p.CategoryId,
                CategoryName = p.Category?.Name
            };
        }

        public async Task AddProduct(Product_DTO dTO)
        {
            var categoryId = dTO.CategoryId ?? throw new Exception("Vui lòng chọn danh mục (Category)!");
            var category = await _categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("Danh mục  này không tồn tại, không thể thêm sản phẩm!");
            }
            bool isCodeExisted = await _productRepo.CheckCodeExisted(dTO.Code);
            if (isCodeExisted)
            {
                throw new Exception("Mã Code sản phẩm này đã có trong hệ thống rồi.");
            }

            var newProduct = new MyApp.Domain.Entities.Product
            {
                Name = dTO.Name,
                Price = dTO.Price,
                Description = dTO.Description,
                CategoryId = categoryId,
                Img = dTO.Img,
                StockQuantity = dTO.StockQuantity,
                Code = dTO.Code,
                RecordStatus = "1"

            };
            await _productRepo.AddAsync(newProduct);
        }

        public async Task UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = await _productRepo.GetByIdProductAsync(id);
            if (product == null) throw new Exception("Sản phẩm này không tồn tại hoặc đã bị xóa mất rồi!");

            if (dto.CategoryId.HasValue)
            {
                var cate = await _categoryRepo.GetByIdAsync(dto.CategoryId.Value);
                if (cate == null) throw new Exception("Danh mục bạn chọn không hợp lệ!");
                product.CategoryId = dto.CategoryId;
            }

            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                bool isCodeUsed = await _productRepo.CheckCodeExistedForOther(dto.Code, id);
                if (isCodeUsed) throw new Exception("Mã Code này đã có sản phẩm khác dùng rồi babe ơi!");
                product.Code = dto.Code.ToUpper();
            }

           /* product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.Img = dto.Img;
            product.StockQuantity = dto.StockQuantity;
            product.UpdatedAt = DateTime.Now;*/
            
            var Product = _mapper.Map(dto, product);
            await _productRepo.UpdateAsync(Product);
        }

        public async Task DeleteProduct(int id)
        {
            if (id <= 0) throw new Exception("Id sản phẩm không hợp lệ babe ơi!");

            var product = await _productRepo.GetByIdProductAsync(id);

            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại hoặc đã bị xóa trước đó rồi.");
            }

            if(product.RecordStatus == "0")
            {
                throw new Exception("sản Phẩm này đã xóa rồi á");
            }
            product.RecordStatus = "0"; 
            product.UpdatedAt = DateTime.Now;

            await _productRepo.UpdateAsync(product);
        }
    }
 }


