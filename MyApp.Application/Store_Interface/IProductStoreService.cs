using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Interface
{
    public interface IProductStoreService
    {
        Task<IEnumerable<Product_DTO>> GetAllProducts();
    
        Task<SpResponse> Create(Product_DTO dto);
        Task<SpResponse> Update(int id, Product_DTO dto);
        Task<SpResponse> Delete(int id);
        Task<IEnumerable<Product_DTO>> GetProductsByCategoryId(int categoryId);
    }
}
