using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<dynamic>> GetListCategoryForUI();

        Task AddCategory(Model_DTO.Category_DTO dTO);
        
        Task UpdateCategory(int id, Model_DTO.CategoryUpdateDto dto);

        Task DeleteCategory(int id);
    }
}
