using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Interface
{
    public interface IPermissionStoreService
    {
        Task<IEnumerable<Permission_DTO>> GetAllPermissionForUI();
        Task<Permission_DTO?> GetPermissionByIdForUI(int id);
        Task<SpResponse> AddPermissionAsync(Permission_DTO permissionDto);
        Task<SpResponse> UpdatePermissionAsync(int id, Permission_DTO permissionDto);
        Task<SpResponse> DeletePermissionAsync(int id);
        Task<SpResponse> AddPermissionToRoleAsync(int roleId, int permissionId);
        Task<SpResponse> UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds);
        Task<IEnumerable<Permission_DTO>> GetPermissionsByRoleIdForUI(int roleId);
        Task<IEnumerable<Role_DTO>> GetRolesByPermissionIdForUI(int permissionId);
    }
}
