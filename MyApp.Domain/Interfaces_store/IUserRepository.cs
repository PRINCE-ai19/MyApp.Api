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
        Task<SpResponse> AddRoleToUser(int userId, int roleId);
        Task<SpResponse> DeleteRoleFromUser(int userId, int roleId);
        Task<IEnumerable<Role>> GetUserRolesDetail(int userId);
        Task<(User user, IEnumerable<Role> roles)> LoginAsync(string usernameOrEmail);
        Task SaveRefreshToken(int userId, string refreshToken, DateTime expires);
        Task<SpResponse> RegisterAsync(User user);
        Task<SpResponse> LogoutAsync(string refreshToken);
        Task<(User user, IEnumerable<Role> roles)> ValidateRefreshToken(string refreshToken);
    }
}
