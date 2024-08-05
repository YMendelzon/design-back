using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;


        public TokenController(ILogger<UserController> logger, IUserService userService, ITokenService tokenService, IConfiguration config)
        {
            _logger = logger;
            _userService = userService;
            _config = config;
            _tokenService = tokenService;
        }

        [HttpGet("ValidateToken")]
        [Authorize]
        public async Task<IActionResult> ValidateAccessToken()
        {
            // קבלת הטוקן מהכותרת Authorization
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            // בדיקת תקינות הטוקן 
            return Ok(_tokenService.ValidateAccessToken(token));
        }
        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Invalid client request");
            }

            // Validate the refresh token
            bool isValidRefreshToken = _tokenService.ValidateRefreshToken(request.RefreshToken);

            if (!isValidRefreshToken)
            {
                return Unauthorized("Invalid refresh token");
            }

            // Generate a new access token
            var email = _tokenService.GetEmailFromAccessToken(request.AccessToken); // Retrieve email from access token if needed
            var role = "user"; // You can retrieve the user's role from your user service or database

            var newAccessToken = _tokenService.BuildAccessToken(role, email);

            return Ok(new { AccessToken = newAccessToken });
        }
    }
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}



