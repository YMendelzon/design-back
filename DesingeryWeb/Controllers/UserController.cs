using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public UserController(ILogger<UserController> logger, IUserService userService, ITokenService tokenService, IConfiguration config)
        {
            _logger = logger;
            _userService = userService;
            _config = config;
            _tokenService = tokenService;
        }


        [HttpGet ("GetUsers")]
        [Authorize (Roles ="3")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
                return _userService.GetAllUsers();
        }

        ///////////////////////////////////////////////////////
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLogin u)
        {
            // אימות המשתמש
            var user = _userService.Login(u.Email, u.PasswordHash);
            if (user == null)
                throw new Exception();

            var tokenService = new TokenService(_config);
            var token = tokenService.BuildToken(
                user.TypeID.ToString(),
                user.Email
            );
            return Ok(new { token });
        }

         [HttpPost("PostUser")]
        public async Task<ActionResult<bool>>PostUser(User u)
        {
            return _userService.PostUser(u);
        }

        [HttpPut("PutUser")]
        [Authorize(Roles = "1,2,3")]
        public async Task<ActionResult<bool>> PutUser(int id, User u)
        {
            return _userService.PutUser(id, u);
        }

        [HttpGet("GetUserDeteils")]
        [Authorize(Roles = "1,2,3")]
        public async Task<ActionResult<User>> GetUserDeteils()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (_tokenService.ValidateToken(token))
            {
                var email = _tokenService.GetEmailFromToken(token);
                if (email != null)
                    return Ok(_userService.GetUserByMail(email));
            }
            return BadRequest();
        }
        [HttpPut("ResetPas")]
        [Authorize(Roles = "1,2,3")]
        public async Task<ActionResult<bool>> ResetPas(string password)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null && _tokenService.ValidateToken(token))
            {
                var email = _tokenService.GetEmailFromToken(token);
                if (email != null)
                    return Ok(_userService.ResetPas(email, password));
            }
            return false;
        }
    }
}

