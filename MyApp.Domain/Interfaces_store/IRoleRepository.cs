using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces_store
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetallRole();

        Task<Role> GetRoleById(int id);

        Task<SpResponse> Add(Role role);

        Task<SpResponse> Update(Role role);

        Task<SpResponse> Delete(int id);
        Task<SpResponse> UpdateUsersInRole(int roleId, IEnumerable<int> userIds);
    }
}
