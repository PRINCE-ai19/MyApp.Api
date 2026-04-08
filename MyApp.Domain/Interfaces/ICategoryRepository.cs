using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        // Trả về dữ liệu thô từ DB
        Task<IEnumerable<dynamic>> GetAllCategoriesAsync();

        Task Add(Category category);

        Task<Category> GetByIdAsync(int id);
        Task UpdateAsync(Category category);


        Task<bool> CheckCodeExisted(string code);

        Task<bool> CheckCodeExistedForOther(string code, int currentId);

        Task<bool> HasRelatedProducts(int categoryId);

        Task<Category> GetByIdAsyncRecordStatus(int id);

    }
}
