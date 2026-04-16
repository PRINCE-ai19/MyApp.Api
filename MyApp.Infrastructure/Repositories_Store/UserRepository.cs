using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
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

        public UserRepository(AppDbContext context, IStringLocalizer<SharedResource> localizer , IStoreHelper storeHelper)
        {
            _context = context;
            _localizer = localizer;
            _storeHelper = storeHelper;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
        
          return await _storeHelper.QueryFirstOrDefaultAsync<User>("sp_GetUserByUsername", new { Username = username });
        }

        public async Task<bool> UpdateUserRefreshTokenAsync(User user)
        {
            _context.Users.Update(user); 
            var result = await _context.SaveChangesAsync();
            return result > 0;
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

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RecordStatus == "1");
        }
    }
}
