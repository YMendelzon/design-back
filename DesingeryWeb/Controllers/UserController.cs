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
        [HttpGet (Name="GetUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            try
            {
                return _userService.GetAllUsers();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
}
        }
       
    }
}
