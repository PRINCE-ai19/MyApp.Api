using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Application.Store_Interface;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Interfaces_store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Store_Services
{
    public class AuthStoreService : IAuthStoreService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtRepository _jwtRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthStoreService> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AuthStoreService(IUserRepository userRepo, IJwtRepository jwtRepo, IMapper mapper , ILogger<AuthStoreService> logger , IStringLocalizer<SharedResource> localizer)
        {
            _userRepo = userRepo;
            _jwtRepo = jwtRepo;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
        {
            var result = await _userRepo.LoginAsync(loginRequest.Username);

            User user = result.user;
            IEnumerable<Role> roles = result.roles;

            if (user == null) return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);

            if (!isPasswordValid) return null;

            var accessToken = _jwtRepo.GenerateAccessToken(user, roles);
            var refreshToken = _jwtRepo.GenerateRefreshToken();

            var refreshTokenExpires = DateTime.Now.AddDays(7);
            await _userRepo.SaveRefreshToken(user.Id, refreshToken, refreshTokenExpires);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<User_DTO>(user)
            };
        }

        public async Task<SpResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return new SpResponse { Success = false, Message = _localizer["passwordwworng"] };
            }

            var user = _mapper.Map<User>(registerRequest);

            return await _userRepo.RegisterAsync(user);
        }

        public async Task<SpResponse> LogoutAsync(string refreshToken)
        {
            return await _userRepo.LogoutAsync(refreshToken);
        }

        public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken)
        {
           
            var (user, roles) = await _userRepo.ValidateRefreshToken(refreshToken);

            if (user == null) return null;

       
            var newAccessToken = _jwtRepo.GenerateAccessToken(user, roles);
            var newRefreshToken = _jwtRepo.GenerateRefreshToken();

      
            var refreshTokenExpires = DateTime.Now.AddDays(7);
            await _userRepo.SaveRefreshToken(user.Id, newRefreshToken, refreshTokenExpires);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                User = _mapper.Map<User_DTO>(user)
            };
        }
    }
}
