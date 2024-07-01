using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IProductService
    {

        bool DeleteProductsCategory(int productId, int cat);
        List<Product> GetAllProducts();
        List<Product> GetProductsByCategory(int categoriId);
        bool PostProduct(Product prod);
        bool PostProductCategory(int proId, int catId);
        bool PutProduct(int prodId, Product prod);


    }
}