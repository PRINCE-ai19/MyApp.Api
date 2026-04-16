using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Services
{
    public interface ICategoryStoreService
    {
        Task<IEnumerable<Category_DTO>> GetAllCategories();
        Task<Category_DTO?> GetById(int id);
        Task<SpResponse> Create(Category_DTO dto);
        Task<SpResponse> Update(int id, Category_DTO dto);
        Task<SpResponse> Delete(int id);

        Task<IEnumerable<Category_DTO>> SearchCategoryByName(string name);
    }
}
