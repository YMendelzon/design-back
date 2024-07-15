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


        //[Authorize]
        [HttpGet ("GetUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
                return _userService.GetAllUsers();
        }

        ///////////////////////////////////////////////////////
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(string mail, string pas)
        {
            // אימות המשתמש
            if (_userService.Login(mail, pas) == null)
                throw new Exception();

            var tokenService = new TokenService(_config);
            var token = tokenService.BuildToken(
                mail,
                _config["Jwt:Key"],
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                Convert.ToDouble(_config["Jwt:ExpiryDurationMinutes"])
            );
            return Ok(new { token });
        }

    [HttpPost("PostUser")]
        public async Task<ActionResult<bool>>PostUser(User u)
        {
            return _userService.PostUser(u);
        }

        [HttpPut("PutUser")]
        public async Task<ActionResult<bool>> PutUser(int id, User u)
        {
            return _userService.PutUser(id, u);
        }
        [HttpGet("GetUserDeteils")]
        //[Authorize]
        public async Task<ActionResult<User>> GetUserDeteils()
        {
            var token = Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();
            if (_tokenService.ValidateToken(token))
            {
                var email = _tokenService.GetEmailFromToken(token);
                if (email != null)
                    return Ok(_userService.GetUserByMail(email));
            }
            return BadRequest();
        }
        [HttpPut("ResetPas")]
        //[Authorize]
        public async Task<ActionResult<bool>> ResetPas(string password)
        {
            var token = Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();
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

