using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static iText.Kernel.Pdf.Colorspace.PdfShading;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

       

        private readonly IProductService _productService;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger,
            IProductService i)
        {
            _productService = i;
            _logger = logger;
        }
        /// <summary>
        ///A function that extracts the FAQ table from the DB
        /// </summary>
        /// <param name="langId">קוד שפה של הטקסט</param>
        /// <returns>מחזיר את כל טבלת FAQ בשפה המבוקשת</returns>
        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            return _productService.GetAllProducts();
        }

        [HttpPost("PostProduct")]
        [Authorize(Roles = "3")]
        public async Task<ActionResult<bool>> PostProduct([FromBody] Product product) { 
            return _productService.PostProduct(product);
        }

        [HttpPut("PutProduct")]

        [Authorize(Roles = "3")]

        public async Task<ActionResult<bool>> PutProduct(int prodID, [FromBody] Product p)

        {
            return _productService.PutProduct(prodID, p);
        }

        [HttpDelete("DeleteProductCategory/{productId}/{cat}")]
        [Authorize(Roles = "3")]

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
        [Authorize(Roles = "3")]

        public async Task<ActionResult<bool>> AddProductCategory(int prodId, int catId)
        {
            return Ok(_productService.PostProductCategory(prodId, catId));
        }

        //[HttpGet("GetRecommendedProducts")]
        //public async Task<ActionResult<List<Product>>> GetRecommendedProducts()
        //{

        //    return _productService.GetRecommendedProducts();
        //}
        [HttpGet("{productId}/categories")]
        public async  Task<ActionResult<List<Categories>>> GetCategoriesForProductId(int productId)
        {
            return Ok(_productService.GetCategoriesHierarchyByProductId(productId));
        }

        [HttpGet("GetSubcategories/{categoryId}")]
        public async Task<ActionResult<List<Categories>>> GetSubCategoriesByCategoryID(int categoryId)
        {
            return Ok(_productService.GetSubcategories(categoryId));
        }

    }
}
