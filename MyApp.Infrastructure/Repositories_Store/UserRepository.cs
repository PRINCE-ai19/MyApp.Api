using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces_store;
using MyApp.Infrastructure.Data.Context;
using MyApp.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repositories_Store
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IStoreHelper _storeHelper;

        public UserRepository(AppDbContext context, IStringLocalizer<SharedResource> localizer, IStoreHelper storeHelper)
        {
            _context = context;
            _localizer = localizer;
            _storeHelper = storeHelper;
        }

        public async Task<IEnumerable<User>> GetallUser()
        {

            return await _storeHelper.QueryAsync<User>("sp_GetAllUsers");
        }
        public async Task<User?> GetByIdAsync(int id)
        {
         return await _storeHelper.QueryFirstOrDefaultAsync<User>("sp_GetUserById", new { Id = id });
        }

        public async Task<SpResponse> Add(User user)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_InsertUser", user);

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

      
        public async Task<SpResponse> UpdateAsync(User user)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_UpdateUser", user);
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }
      

        public async Task<SpResponse> DeleteAsync(int id)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_DeleteUser", new { Id = id });
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }
            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> AddRoleToUser(int userId, int roleId)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_AddRoleToUser", new { UserId = userId, RoleId = roleId });
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }
            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> DeleteRoleFromUser(int userId, int roleId)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_DeleteRoleFromUser", new { UserId = userId, RoleId = roleId });
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }
            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<IEnumerable<Role>> GetUserRolesDetail(int userId)
        {
            return await _storeHelper.QueryAsync<Role>("sp_GetUserRolesDetail", new { UserId = userId });
        }

        public async Task<(User user, IEnumerable<Role> roles)> LoginAsync(string usernameOrEmail)
        {
            using (var multi = await _storeHelper.QueryMultipleAsync("sp_Login", new { Email = usernameOrEmail }))
            {
                var user = await multi.ReadFirstOrDefaultAsync<User>();
                var roles = (await multi.ReadAsync<Role>()).ToList(); 
                return (user, roles);
            }
        }
        public async Task SaveRefreshToken(int userId, string refreshToken, DateTime expires)
        {
            await _storeHelper.ExecuteAsync("sp_SaveRefreshToken", new 
            { 
                UserId = userId, 
                RefreshToken = refreshToken, 
                Expires = expires 
            });
        }

        public async Task<SpResponse> RegisterAsync(User user)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_RegisterUser", user);

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> LogoutAsync(string refreshToken)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_Logout", new { RefreshToken = refreshToken });

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<(User user, IEnumerable<Role> roles)> ValidateRefreshToken(string refreshToken)
        {
            using (var reader = await _storeHelper.QueryMultipleAsync("sp_ValidateRefreshToken", new { RefreshToken = refreshToken }))
            {
                var user = await reader.ReadFirstOrDefaultAsync<User>();
                var roles = await reader.ReadAsync<Role>();
                return (user!, roles);
            }
        }
    }
}
