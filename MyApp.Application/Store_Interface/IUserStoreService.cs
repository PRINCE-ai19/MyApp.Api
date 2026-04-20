using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Interface
{
    public interface IUserStoreService
    {
            Task<IEnumerable<User_DTO>> GetListUserForUI();
            Task<User_DTO?> GetUserByIdForUI(int id);
            Task<SpResponse> AddUserAsync(UserADD_DTO userDto);
            Task<SpResponse> UpdateUserAsync(int id , User_DTO userDto);
            Task<SpResponse> DeleteUserAsync(int id);
            Task<SpResponse> AddRoleToUserAsync(int userId, int roleId);
            Task<SpResponse> DeleteRoleFromUserAsync(int userId, int roleId);
            Task<IEnumerable<Role_DTO>> GetUserRolesDetailForUI(int userId);
    }
}
