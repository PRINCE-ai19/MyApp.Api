using MyApp.Domain.Entities;
using MyApp.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces_store
{
    public interface ICategoryRepository_store
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<SpResponse> UpdateAsync(Category category);

        Task<SpResponse> AddAsync(Category category);

        Task<SpResponse> DeleteAsync(int id);
    }
}
