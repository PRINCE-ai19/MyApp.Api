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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IStoreHelper _storeHelper;
        public PermissionRepository(AppDbContext context, IStringLocalizer<SharedResource> localizer, IStoreHelper storeHelper)
        {
            _context = context;
            _localizer = localizer;
            _storeHelper = storeHelper;
        }

        public async Task<IEnumerable<Permission>> GetallPermission()
        {
            return await _storeHelper.QueryAsync<Permission>("sp_GetAllPermissions");
        }
        public async Task<Permission?> GetPermissionById(int id)
        {
            return await _storeHelper.QueryFirstOrDefaultAsync<Permission>("sp_GetPermissionById", new { Id = id });
        }
        public async Task<SpResponse> Add(Permission permission)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_InsertPermission", permission);

            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> Delete(int id)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_DeletePermission", new { Id = id });
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }
            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }


        public async Task<SpResponse> Update(Permission permission)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_UpdatePermission", permission);
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }

            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }
    }
}
