using MyApp.Application.Model_DTO;
using MyApp.Domain.Interfaces;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;

namespace MyApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtRepository _jwtService;

        public AuthService(IUserRepository userRepo, IJwtRepository jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
      
            var user = await _userRepo.GetUserByUsernameAsync(request.Username);

       
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHass))
            {
                throw new Exception("Tài khoản hoặc mật khẩu không đúng!");
            }

            // 3. Tạo cặp Token
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // 4. Cập nhật Refresh Token vào User (Logic LINQ đã có trong Repo)
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userRepo.UpdateUserRefreshTokenAsync(user);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username
            };
        }

        public async Task<SpResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                PasswordHass = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                Email = request.Email,
                Role = "User"
            };

            return await _userRepo.RegisterAsync(user);
        }
    }
}
