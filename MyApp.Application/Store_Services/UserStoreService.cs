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
    public class UserStoreService : IUserStoreService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UserStoreService(IUserRepository repo , IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User_DTO>> GetListUserForUI()
        {
            var entities = await _repo.GetallUser();
            return _mapper.Map<IEnumerable<User_DTO>>(entities);
        }

        public async Task<User_DTO?> GetUserByIdForUI(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<User_DTO>(entity);
        }

        public async Task<SpResponse> AddUserAsync(UserADD_DTO userDto)
        {
            var entity = _mapper.Map<User>(userDto);
            return await _repo.Add(entity);
        }

        public async Task<SpResponse> UpdateUserAsync(int id ,User_DTO userDto)
        {
            var entity = _mapper.Map<User>(userDto);
            entity.Id = id;
            return await _repo.UpdateAsync(entity);
        }

        public async Task<SpResponse> DeleteUserAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<SpResponse> AddRoleToUserAsync(int userId, int roleId)
        {
            return await _repo.AddRoleToUser(userId, roleId);
        }

        public async Task<SpResponse> DeleteRoleFromUserAsync(int userId, int roleId)
        {
            return await _repo.DeleteRoleFromUser(userId, roleId);
        }

        public async Task<IEnumerable<Role_DTO>> GetUserRolesDetailForUI(int userId)
        {
            var entities = await _repo.GetUserRolesDetail(userId);
            return _mapper.Map<IEnumerable<Role_DTO>>(entities);
        }
    }
}
