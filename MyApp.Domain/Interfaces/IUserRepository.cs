using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces
{
    public interface IUserRepository
    {
            Task<User?> GetUserByUsernameAsync(string username);
            
            Task<bool> UpdateUserRefreshTokenAsync(User user);

            Task<SpResponse> RegisterAsync(User user);

           Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

    }
}
