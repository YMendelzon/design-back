using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [HttpGet ("GetUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
                return _userService.GetAllUsers();
        }
        [HttpGet("Login/{mail}/{pas}")]
        public  async Task<ActionResult<User>> Login(string mail, string pas)
        {
            return _userService.Login(mail, pas);

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
    }
}
