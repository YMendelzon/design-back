﻿//using DesigneryCommon.Model
//using DesigneryCore.Interfaces;
//using DesigneryCore.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace DesingeryWeb.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductController : ControllerBase
//    {



//        private readonly IProductService _productService;

//        private readonly ILogger<ProductController> _logger;

//        public ProductController(ILogger<ProductController> logger,
//            IProductService i)
//        {
//            _productService = i;
//            _logger = logger;
//        }
//        /// <summary>
//        ///A function that extracts the FAQ table from the DB
//        /// </summary>
//        /// <param name="langId">קוד שפה של הטקסט</param>
//        /// <returns>מחזיר את כל טבלת FAQ בשפה המבוקשת</returns>
//        [HttpGet("GetAllProduct")]
//        public async Task<ActionResult<List<Product>>> GetAllProduct()
//        {
//            return _productService.GetAllProducts();
//        }

//        [HttpPost("PostProduct")]
// [Authorize(Roles = "3")]
//        public async Task<ActionResult<bool>> PostProduct([FromBody] Product product) { 
//            return _productService.PostProduct(product);
//        }

//        [HttpPut("PutProduct")]

//        [Authorize(Roles = "3")]

//        public async Task<ActionResult<bool>> PutProduct(int prodID, [FromBody] Product p)

//        {
//            return _productService.PutProduct(prodID, p);
//        }

//        [HttpDelete("DeleteProductCategory/{productId}/{cat}")]
//        [Authorize(Roles = "3")]

//        public async Task<ActionResult<bool>> DeleteProductCategory(int productId, int cat) 
//        {
//            return _productService.DeleteProductsCategory(productId, cat);
//        }

//        [HttpGet("GetProductByCategory/{categoriId}")]
//        [Authorize(Roles = "3")]

//        public async Task<ActionResult<List<Product>>> GetProductByCategory(int categoriId)
//        {
//            return _productService.GetProductsByCategory(categoriId);
//        }

//        [HttpPost("AddProductCategory/{prodId}/{catId}")]
//        [Authorize(Roles = "3")]

//        public async Task<ActionResult<bool>> AddProductCategory(int prodId, int catId)
//        {
//            return Ok(_productService.PostProductCategory(prodId, catId));
//        }

//        //[HttpGet("GetRecommendedProducts")]
//        //public async Task<ActionResult<List<Product>>> GetRecommendedProducts()
//        //{

//        //    return _productService.GetRecommendedProducts();
//        //}
//    }
//}
using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using DesigneryDAL;
using iText.Commons.Actions.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using static iText.Kernel.Pdf.Colorspace.PdfShading;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly S3Service _s3Service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService, S3Service s3Service)
        {
            _productService = productService;
            _s3Service = s3Service;
            _logger = logger;
        }


        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            return _productService.GetAllProducts();
        }

        [HttpDelete("DeleteProductCategory/{productId}/{cat}")]

        public async Task<ActionResult<bool>> DeleteProductCategory(int productId, int cat) 
        {
            return _productService.DeleteProductsCategory(productId, cat);
        }

        [HttpGet("GetProductByCategory/{categoriId}")]
        public async Task<ActionResult<List<Product>>> GetProductByCategory(int categoriId)
        {
            return _productService.GetProductsByCategory(categoriId);
        }

        [HttpPost("AddProductCategory/{prodId}/{catId}")]
        public async Task<ActionResult<bool>> AddProductCategory(int prodId, int catId)
        {
            return Ok(_productService.PostProductCategory(prodId, catId));
        }

        [HttpPost("PostProduct")]
        public async Task<ActionResult<bool>> CreateProduct([FromForm] Product product)
        {
            if (product.Image != null)
            {
                var imageUrl = await _s3Service.UploadFileAsync(product.Image);
                product.ImageURL = imageUrl;
            }
            return _productService.PostProduct(product);
        }

        [HttpPut("PutProduct")]
        public async Task<bool>  PutProduct([FromForm] Product p)
        {
            List<NpgsqlParameter> getImageUrlParams = new List<NpgsqlParameter> {
                  new NpgsqlParameter("p_id", p.ProductID)
            };

            var imageUrlResult = DataAccessPostgreSQL.ExecuteFunction<List<string>>("GetProductImageURL", getImageUrlParams);

            if (p.Image != null)
            {
                var imageUrll = await _s3Service.UploadFileAsync(p.Image);
                p.ImageURL = imageUrll;
            }

            return _productService.PutProduct(p);
        }

        [HttpGet("{productId}/categories")]
        public async  Task<ActionResult<List<Categories>>> GetCategoriesForProductId(int productId)
        {
            return Ok(_productService.GetCategoriesHierarchyByProductId(productId));
        }

        [HttpGet("GetProductsByCategoryAndSubcategories/{categoryId}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategoryAndSubcategories(int categoryId)
        {
            return Ok(_productService.GetProductsByCategoryAndSubcategories(categoryId));
        }
    }
}
 