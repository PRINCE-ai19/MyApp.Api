using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Application.Store_Interface;
using System.Threading.Tasks;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthStoreService _authService;
         private readonly ILogger<AuthController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AuthController(IAuthStoreService authService , ILogger<AuthController> logger , IStringLocalizer<SharedResource> localizer )
        {
            _authService = authService;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authService.LoginAsync(loginRequest);

            if (result == null)
            {
                return Unauthorized(new { succcess = 0 , Message = _localizer["wrong_login"] });
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await _authService.RegisterAsync(registerRequest);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] string refreshToken)
        {
            var result = await _authService.LogoutAsync(refreshToken);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (result == null)
            {
                return Unauthorized(new { success = 0 , Message = _localizer["Session_Worng"]  });
            }

            return Ok(result);
        }
    }
}