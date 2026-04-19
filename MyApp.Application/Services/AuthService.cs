/*using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtRepository _jwtService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepo, IJwtRepository jwtService, IStringLocalizer<SharedResource> localizer, IMapper mapper, ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _localizer = localizer;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
      
            var user = await _userRepo.GetUserByUsernameAsync(request.Username);

       
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHass))
            {
                _logger.LogError(_localizer["InvalidCredentials"]);
            }

            if (user.RecordStatus != "1")
            {
                _logger.LogError(_localizer["AccountLocked"]);
            }

            //  Tạo cặp Token
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Cập nhật Refresh Token vào User (Logic LINQ đã có trong Repo)
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
            *//*var user = new User
            {
                Username = request.Username,
                PasswordHass = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                Email = request.Email,
                Role = "User"
            };

            return await _userRepo.RegisterAsync(user);*//*
            var user  = _mapper.Map<User>(request);
            return await _userRepo.RegisterAsync(user);
        }

        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepo.GetUserByRefreshTokenAsync(refreshToken);

            if(user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _logger.LogError(_localizer["expertoken"]);
            }
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userRepo.UpdateUserRefreshTokenAsync(user);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Username = user.Username
            };
        }
    }
}
*/