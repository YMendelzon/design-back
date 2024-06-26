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
        [HttpGet("GetAllProducts/langId")]
        public async Task<ActionResult<List<Products>>> GetAllProducts(int langId)
        {
            return _productService.GetAllProducts(langId);
        }


        // נשמע לי מטורף לשלוח כרגע פרמטרים - ומצד שני אי אפשר לעשות את זה בתור אובייקט - בגלל העברית - אנגלית
        [HttpPost("PostProduct/NameH/DescriptionH/NameE/DescriptionE/Price/ImageURL/SalePrice")]
        public async Task<ActionResult<bool>> PostProduct(string NameH, string DescriptionH, string NameE, string DescriptionE, decimal Price, string ImageURL, decimal SalePrice)
        {

            return _productService.PostProduct(NameH, DescriptionH, NameE, DescriptionE, Price, ImageURL, SalePrice);
        }

        [HttpPut("PutProduct/id/NameH/DescriptionH/NameE/DescriptionE/Price/ImageURL/SalePrice")]
        public async Task<ActionResult<bool>> PutProduct(int id, string NameH, string DescriptionH, string NameE, string DescriptionE, decimal Price, string ImageURL, decimal SalePrice)
        {
            return _productService.PutProduct(id, NameH, DescriptionH, NameE, DescriptionE, Price, ImageURL, SalePrice);
        }
    }
}
