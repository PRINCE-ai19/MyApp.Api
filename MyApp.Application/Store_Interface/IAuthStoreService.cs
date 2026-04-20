using MyApp.Application.Model_DTO;
using MyApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Interface
{
    public interface IAuthStoreService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest loginRequest);
        Task<SpResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<SpResponse> LogoutAsync(string refreshToken);
        Task<LoginResponse?> RefreshTokenAsync(string refreshToken);
    }
}
