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
        //[Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            // קבלת הטוקן מהכותרת Authorization
            var token = Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            // בדיקת תקינות הטוקן 
            return Ok(_tokenService.ValidateToken(token));
        }
        //[Authorize]
        [HttpGet("GetUserDeteils")]
        public async Task<ActionResult<User>> GetUserDeteils()
        {
            var token = Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();
            if(_tokenService.ValidateToken(token))
            {
                var email = _tokenService.GetEmailFromToken(token);
                if (email != null)
                    return Ok(_userService.GetUserByMail(email));
            }
            return BadRequest();
        }


 

    }
}



