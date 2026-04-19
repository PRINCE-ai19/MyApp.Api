using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces_store
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetallPermission();

        Task<Permission> GetPermissionById(int id);

        Task<SpResponse> Add(Permission permission);

        Task<SpResponse> Update(Permission permission);

        Task<SpResponse> Delete(int id);
    }
}
