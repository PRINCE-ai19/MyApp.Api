using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces_store
{
    public interface IUserRepository
    {
            Task<IEnumerable<User>> GetallUser();

            Task<SpResponse> Add(User user);

            Task<User> GetByIdAsync(int id);

            Task<SpResponse> UpdateAsync(User user);

            Task<SpResponse> DeleteAsync(int id);

    }
}
