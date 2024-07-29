using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IProductService
    {

        List<Product> GetAllProducts();
        bool PostProduct(Product prod);
        bool PutProduct(int prodId, Product prod);
        List<Product> GetProductsByCategory(int categoriId);
        bool PostProductCategory(int proId, int catId);
        bool DeleteProductsCategory(int productId, int cat);
        List<Categories> GetCategoriesHierarchyByProductId(int productId);
        List<Categories> GetSubcategories(int categoryId);
        List<Product> GetProductsByCategoryAndSubcategories(int categoryId);
        //List<Product> GetRecommendedProducts();



    }
}