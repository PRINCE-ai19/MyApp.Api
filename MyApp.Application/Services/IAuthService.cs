using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<SpResponse> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    }
}
