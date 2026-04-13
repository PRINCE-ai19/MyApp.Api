using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepo , ICategoryRepository categoryRepo , IMapper mapper , IStringLocalizer<SharedResource> localizer , ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<IEnumerable<dynamic>> GetProductsByCategoryId(int catId)
        {
            var products = await _productRepo.GetByCategoryIdAsync(catId);


            return _mapper.Map<IEnumerable<ProductDetailDTO>>(products);
        }

        public async Task<IEnumerable<dynamic>> GetAllProducts()
        {
            var products = await _productRepo.GetAllProductAsync();

            return _mapper.Map<IEnumerable<ProductDetailDTO>>(products);
        }

        public async Task<dynamic?> GetProductById(int id)
        {
            var p = await _productRepo.GetByIdProductAsync(id);
            if (p == null) return null;

            return _mapper.Map<ProductDetailDTO>(p);
        }

        public async Task AddProduct(Product_DTO dTO)
        {
            var categoryId = dTO.CategoryId ?? throw new Exception(_localizer["CategoryRequired"]);
            var category = await _categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogError(_localizer["CategoryNotFound"]);
            }
            bool isCodeExisted = await _productRepo.CheckCodeExisted(dTO.Code);
            if (isCodeExisted)
            {
                _logger.LogError(_localizer["DuplicateProductCode"]);
            }

           /*  var newProduct = new MyApp.Domain.Entities.Product
            {
                Name = dTO.Name,
                Price = dTO.Price,
                Description = dTO.Description,
                CategoryId = categoryId,
                Img = dTO.Img,
                StockQuantity = dTO.StockQuantity,
                Code = dTO.Code,
                RecordStatus = "1"

            };*/
           
            var newProduct = _mapper.Map<Product>(dTO);
            newProduct.Code = dTO.Code.Trim().ToUpper();
            await _productRepo.AddAsync(newProduct);
        }

        public async Task UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = await _productRepo.GetByIdProductAsync(id);
            if (product == null) _logger.LogError(_localizer["ProductAlreadyDeleted"]);

            if (dto.CategoryId.HasValue)
            {
                var cate = await _categoryRepo.GetByIdAsync(dto.CategoryId.Value);
                if (cate == null) _logger.LogError(_localizer["InvalidCategory"]);
                product.CategoryId = dto.CategoryId;
            }

            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                bool isCodeUsed = await _productRepo.CheckCodeExistedForOther(dto.Code, id);
                if (isCodeUsed) _logger.LogError(_localizer["DuplicateProductCode"]);
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
            if (id <= 0) _logger.LogError(_localizer["InvalidProductId"]);

            var product = await _productRepo.GetByIdProductAsync(id);

            if (product == null)
            {
               _logger.LogError(_localizer["ProductNotFound"]);
            }

            if(product.RecordStatus == "0")
            {
                _logger.LogError(_localizer["ProductAlreadyDeleted"]);
            }
            product.RecordStatus = "0"; 
            product.UpdatedAt = DateTime.Now;

            await _productRepo.UpdateAsync(product);
        }
    }
 }


