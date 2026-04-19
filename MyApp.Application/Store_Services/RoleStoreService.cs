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
    public class RoleStoreService : IRoleSoteService
    {
        private readonly IRoleRepository _repo;
        private readonly IMapper _mapper;
        public RoleStoreService(IRoleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Role_DTO>> GetListRoleForUI()
        {
            var entities = await _repo.GetallRole();
            return _mapper.Map<IEnumerable<Role_DTO>>(entities);
        }

        public async Task<Role_DTO?> GetRoleByIdForUI(int id)
        {
            var entity = await _repo.GetRoleById(id);
            return entity == null ? null : _mapper.Map<Role_DTO>(entity);
        }

        public async Task<SpResponse> AddRoleAsync(Role_DTO roleDto)
        {
            var entity = _mapper.Map<Role>(roleDto);
            return await _repo.Add(entity);
        }

        public async Task<SpResponse> UpdateRoleAsync(int id, Role_DTO roleDto)
        {
            var entity = _mapper.Map<Role>(roleDto);
            entity.Id = id;
            return await _repo.Update(entity);
        }

        public async Task<SpResponse> DeleteRoleAsync(int id)
        {
            return await _repo.Delete(id);
        }

      
    }
}
