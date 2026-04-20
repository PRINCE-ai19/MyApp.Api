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
    public interface IRoleSoteService
    {
       Task<IEnumerable<Role_DTO>> GetListRoleForUI();
       Task<Role_DTO?> GetRoleByIdForUI(int id);
       Task<SpResponse> AddRoleAsync(Role_DTO roleDto);
       Task<SpResponse> UpdateRoleAsync( int id , Role_DTO roleDto);
       Task<SpResponse> DeleteRoleAsync(int id);
       Task<SpResponse> UpdateUsersInRoleAsync(int roleId, IEnumerable<int> userIds);
    }
}
