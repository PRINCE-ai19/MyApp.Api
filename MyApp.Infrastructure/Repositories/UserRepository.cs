using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data.Context;
//using MyApp.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UserRepository(AppDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.RecordStatus == "1");
        }

        public async Task<bool> UpdateUserRefreshTokenAsync(User user)
        {
            _context.Users.Update(user); 
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<SpResponse> RegisterAsync(User user)
        {
            var connection = _context.Database.GetDbConnection();
            
          
           // var parameters = await DapperHelper.MapParametersAsync(connection, "sp_RegisterUser", user);

            var response = await connection.QueryFirstOrDefaultAsync<SpResponse>(
                "sp_RegisterUser",
              //  parameters,
                commandType: CommandType.StoredProcedure
            );

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }
    }
}
