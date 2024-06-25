using DesigneryCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesingeryWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;

        public AdminController(ILogger<AdminController> logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        [HttpGet(Name = "Example")]
        public async Task<ActionResult<int>> Example()
        {
            try
            {
               return _adminService.Get();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
