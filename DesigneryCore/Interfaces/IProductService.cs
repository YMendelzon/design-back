using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IProductService
    {
        bool DeleteProductsCategory(int productId, int cat);
        List<Products> GetAllProducts();
        List<Products> GetProductsByCategory(int categoriId);
        bool PostProduct(Products prod);
        bool PostProductCategory(int proId, int catId);
        bool PutProduct(int prodId, Products prod);
    }
}