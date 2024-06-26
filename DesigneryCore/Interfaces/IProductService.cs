using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IProductService
    {
        bool DeleteProductsCategory(int productId, int cat);
        List<Products> GetAllProducts(int lang);
        List<Products> GetProductsByCategory(int categoriId, int lang);
        bool PostProduct(string NameH, string DescriptionH, string NameE, string DescriptionE, decimal Price, string ImageURL, decimal SalePrice);
        bool PutProduct(int id, string NameH, string DescriptionH, string NameE, string DescriptionE, decimal Price, string ImageURL, decimal SalePrice);
    }
}