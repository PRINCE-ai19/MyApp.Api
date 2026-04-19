using Microsoft.Extensions.Localization;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces_store;
using MyApp.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repositories_Store
{
    public class RoleRepository : IRoleRepository
    {

        private readonly AppDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IStoreHelper _storeHelper;
        public RoleRepository(AppDbContext context, IStringLocalizer<SharedResource> localizer, IStoreHelper storeHelper)
        {
            _context = context;
            _localizer = localizer;
            _storeHelper = storeHelper;
        }

        public async Task<IEnumerable<Role>> GetallRole()
        {
            return await _storeHelper.QueryAsync<Role>("sp_GetAllRoles");
        }

        public async Task<Role?> GetRoleById(int id)
        {
            return await _storeHelper.QueryFirstOrDefaultAsync<Role>("sp_GetRoleById", new { Id = id });
        }

        public async Task<SpResponse> Add(Role role)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_InsertRole", role);

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }


        public async Task<SpResponse> Update(Role role)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_UpdateRole", role);
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> Delete(int id)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_DeleteRole", new { Id = id });
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }
            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }


      
    }
}
