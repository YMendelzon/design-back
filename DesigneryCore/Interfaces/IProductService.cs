using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IProductService
    {
        bool DeleteProductCategory(int productId, int cat);
        List<Product> GetAllProduct();
        List<Product> GetProductByCategory(int categoriId);
        bool PostProduct(Product prod);
        bool PutProduct(int prodId, Product prod);
    }
}