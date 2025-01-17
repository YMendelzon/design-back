﻿using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using DesigneryDAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly S3Service _s3Service;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoriesService i, S3Service s3Service)
        {
            _categoriesService = i;
            _s3Service = s3Service;
            _logger = logger;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<List<Categories>>> GetAllCategories()
        {
            return _categoriesService.GetAllCategories();
        }

        [HttpGet("GetCategoryById/{categoryId}")]
        public async Task<ActionResult<Categories>> GetCategory(int categoryId)
        {
            return _categoriesService.GetCategoryById(categoryId);
        }

        //[HttpGet("GetUpCategoriesByCategoryID/{categoryId}")]
        //public async Task<ActionResult<Categories>> GetUpCategoriesByCategoryID(int categoryId)
        //{
        //    return Ok(_categoriesService.GetUpCategoriesByCategoryID(categoryId));
        //}

        [HttpGet("GetSubcategories/{categoryId}")]
        public async Task<ActionResult<List<Categories>>> GetSubCategoriesByCategoryID(int categoryId)
        {
            return Ok(_categoriesService.GetSubcategories(categoryId));
        }

        [HttpPost("AddCategory")]
        //[Authorize(Roles = "3")]
        public async Task<ActionResult<bool>> AddCategory(Categories category)
        {
            if (category == null)
            {
                return BadRequest("Category is null");
            }
            if (category.Image != null)
            {
                var imageUrl = await _s3Service.UploadFileAsync(category.Image);
                category.ImageURL = imageUrl;
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
            List<NpgsqlParameter> getImageUrlParams = new List<NpgsqlParameter> {
                  new NpgsqlParameter("p_id", category.CategoryID)
            };

            var imageUrlResult = DataAccessPostgreSQL.ExecuteFunction<List<string>>("GetProductImageURL", getImageUrlParams);
            if (category.Image != null)
            {
                var imageUrl = await _s3Service.UploadFileAsync(category.Image);
                category.ImageURL = imageUrl;
            }
            var result = _categoriesService.PutCategories(id, category);
            return Ok(true);
        }
    }
}
