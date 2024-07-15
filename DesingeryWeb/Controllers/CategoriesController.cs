using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoriesService i)
        {
            _categoriesService = i;
            _logger = logger;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<List<Categories>>> GetAllCategories()
        {
            return _categoriesService.GetAllCategories();
        }

       
        [HttpPost("AddCategory")]
        public async Task<ActionResult<bool>> AddCategory(Categories category)
        {
            if (category == null)
            {
                return BadRequest("Category is null");
            }

            var result = _categoriesService.postCategories(category);
            return Ok(true);
        }


        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult<bool>> UpdateCategory(int id,Categories category)
        {
            if (category == null)
            {
                return BadRequest("Category is null");
            }

            var result = _categoriesService.PutCategories(id, category);
            return Ok(true);
        }
    }
}
