using DesigneryCommon.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesigneryCore.Interfaces
{
    public interface IProductService
    {

        List<Product> GetProducts();
        bool PostProduct(Product product);
        //bool PutProduct(int prodId, Product prod);
        List<Product> GetProductsByCategory(int categoriId);
        bool PostProductCategory(int proId, int catId);
        bool DeleteProductsCategory(int productId, int cat);
        bool DeleteProduct(int productId);
        //Task PutProduct(Product p);
        Task<bool> PutProduct(Product p);
    }
}