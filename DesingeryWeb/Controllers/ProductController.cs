﻿using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
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
        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            return _productService.GetAllProduct();
        }


   
        [HttpPost("PostProduct")]
        public async Task<ActionResult<bool>> PostProduct(Product product)
        {
            return _productService.PostProduct(product);
        }


        [HttpPut("PutProduct/{prodID}")]
        public async Task<ActionResult<bool>> PutProduct(int prodID, Product p)
        {
            return _productService.PutProduct(prodID, p);
        }
        [HttpDelete("Delete/{productId}/{cat}")]
        public async Task<ActionResult<bool>> DeleteProduct(int productId, int cat) 
        {
            return _productService.DeleteProductCategory(productId, cat);
        }
        [HttpGet("GetProductByCategory.{categoriId}")]
        public async Task<ActionResult<List<Product>>> GetProductByCategory(int categoriId)
        {
            return _productService.GetProductByCategory(categoriId);
        }
    }
}
