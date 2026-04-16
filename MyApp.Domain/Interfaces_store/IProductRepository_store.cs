using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces_store
{
    public interface IProductRepository_store
    {
        Task<IEnumerable<Product>> GetAllProductAsync();

        Task<Product> GetByIdAsync(int id); 
        Task<SpResponse> UpdateAsync(Product product);

        Task<SpResponse> AddAsync(Product product);

        Task<SpResponse> DeleteAsync(int id);

        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    }
}
