using Microsoft.Extensions.Localization;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces_store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repositories_Store
{
    public class ProductRepository_store : IProductRepository_store
    {
        private readonly IStoreHelper _storeHelper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public ProductRepository_store(IStoreHelper storeHelper, IStringLocalizer<SharedResource> localizer)
        {
            _storeHelper = storeHelper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _storeHelper.QueryAsync<Product>("sp_GetAllProducts");
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _storeHelper.QueryFirstOrDefaultAsync<Product>("sp_GetProductById", new { Id = id });
        }

        public async Task<SpResponse> AddAsync(Product product)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_InsertProduct", product);

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> UpdateAsync(Product product)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_UpdateProduct", product);

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> DeleteAsync(int id)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_DeleteProduct", new { Id = id });

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _storeHelper.QueryAsync<Product>("sp_GetProductsByCategoryId", new { CategoryId = categoryId });
        }
    }
}
