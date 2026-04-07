using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);

        Task<IEnumerable<dynamic>> GetAllProductAsync();

        Task Add(Product product);

        Task<Product> GetByIdProductAsync(int id);
        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}
