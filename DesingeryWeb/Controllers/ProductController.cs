using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<List<Products>>> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }


        [HttpPost("PostProduct")]
        public async Task<ActionResult<bool>> PostProduct(Products p)
        {
            return _productService.PostProduct(p);
        }

        [HttpPut("PutProduct/{prodID}")]
        public async Task<ActionResult<bool>> PutProduct(int prodID, Products p)
        {
            return _productService.PutProduct(prodID, p);
        }
        [HttpDelete("Delete/{productId}/{cat}")]
        public async Task<ActionResult<bool>> DeleteProduct(int productId, int cat) 
        {
            return _productService.DeleteProductsCategory(productId, cat);
        }
        [HttpGet("GetProductsByCategory.{categoriId}")]
        public async Task<ActionResult<List<Products>>> GetProductsByCategory(int categoriId)
        {
            return _productService.GetProductsByCategory(categoriId);
        }
    }
}
