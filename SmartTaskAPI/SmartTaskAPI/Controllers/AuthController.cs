using Microsoft.AspNetCore.Mvc;
using SmartTaskAPI.DTOs.Auth;
using SmartTaskAPI.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace SmartTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IAuthService _authService;


        public AuthController(IAuthService auth)
        {
            _auth = auth;
            _authService = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _auth.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);

            //  Only add cookie if token is not null or empty
            if (!string.IsNullOrWhiteSpace(response.Token))
            {
                Response.Cookies.Append("access_token", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(12)
                });
            }

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Overwrite the access_token cookie with an expired one
            Response.Cookies.Append("access_token", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Logged out successfully." });
        }

    }
}
