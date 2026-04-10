using MyApp.Application.Model_DTO;
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
}
