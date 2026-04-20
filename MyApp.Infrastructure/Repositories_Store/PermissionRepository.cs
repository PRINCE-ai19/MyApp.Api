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

        public async Task<SpResponse> AddPermissionToRole(int roleId, int permissionId)
        {
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_AddPermissionToRole", new { RoleId = roleId, PermissionId = permissionId });
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }
            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<SpResponse> UpdateRolePermissions(int roleId, IEnumerable<int> permissionIds)
        {
            var ids = string.Join(",", permissionIds);
            var response = await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_UpdateRolePermissions", new { RoleId = roleId, PermissionIds = ids });
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message = _localizer[response.Message];
            }
            return response ?? new SpResponse { Success = false, Message = _localizer["ERROR_UNKNOWN_DATABASE"] };
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleId(int roleId)
        {
            return await _storeHelper.QueryAsync<Permission>("sp_GetPermissionsByRoleId", new { RoleId = roleId });
        }

        public async Task<IEnumerable<Role>> GetRolesByPermissionId(int permissionId)
        {
            return await _storeHelper.QueryAsync<Role>("sp_GetRolesByPermissionId", new { PermissionId = permissionId });
        }
    }
}
