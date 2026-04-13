using MyApp.Domain.Common;
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

        Task<SpResponse> AddCategory(Model_DTO.Category_DTO dTO);
        
        Task<SpResponse> UpdateCategory(int id, Model_DTO.CategoryUpdateDto dto);

        Task<SpResponse> DeleteCategory(int id);
    }
}
