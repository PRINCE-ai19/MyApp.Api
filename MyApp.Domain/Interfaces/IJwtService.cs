using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces
{
    public interface IJwtRepository
    {
        string GenerateAccessToken(User user , IEnumerable<Role> roles);
        string GenerateRefreshToken();
    }
}
