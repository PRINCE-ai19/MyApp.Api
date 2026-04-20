using AutoMapper;
using MyApp.Application.Model_DTO;
using MyApp.Application.Store_Interface;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces_store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Services
{
    public class PermissionStoreService : IPermissionStoreService
    {
        private readonly IPermissionRepository _repo;
        private readonly IMapper _mapper;
        public PermissionStoreService(IPermissionRepository repo , IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Permission_DTO>> GetAllPermissionForUI()
        {
            var permissions = await _repo.GetallPermission();
            return _mapper.Map<IEnumerable<Permission_DTO>>(permissions);
        }

        public async Task<Permission_DTO?> GetPermissionByIdForUI(int id)
        {
            var permission = await _repo.GetPermissionById(id);
            return _mapper.Map<Permission_DTO>(permission);
        }

        public async Task<SpResponse> AddPermissionAsync(Permission_DTO permissionDto)
        {
            var permission = _mapper.Map<Permission>(permissionDto);
            return await _repo.Add(permission);
        }

        public async Task<SpResponse> UpdatePermissionAsync(int id, Permission_DTO permissionDto)
        {
            var permission = _mapper.Map<Permission>(permissionDto);
            permission.Id = id;
            return await _repo.Update(permission);
        }

        public async Task<SpResponse> DeletePermissionAsync(int id)
        {
            return await _repo.Delete(id);
        }

        public async Task<SpResponse> AddPermissionToRoleAsync(int roleId, int permissionId)
        {
            return await _repo.AddPermissionToRole(roleId, permissionId);
        }

        public async Task<SpResponse> UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds)
        {
            return await _repo.UpdateRolePermissions(roleId, permissionIds);
        }

        public async Task<IEnumerable<Permission_DTO>> GetPermissionsByRoleIdForUI(int roleId)
        {
            var permissions = await _repo.GetPermissionsByRoleId(roleId);
            return _mapper.Map<IEnumerable<Permission_DTO>>(permissions);
        }

        public async Task<IEnumerable<Role_DTO>> GetRolesByPermissionIdForUI(int permissionId)
        {
            var roles = await _repo.GetRolesByPermissionId(permissionId);
            return _mapper.Map<IEnumerable<Role_DTO>>(roles);
        }
    }
}
