using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DesigneryCore.Services
{
    public class ProductService : IProductService
    {
        public List<Products> GetAllProducts(int lang)
        {
            try
            {
                SqlParameter langParam = new SqlParameter("@Lang", lang);
                //called the function from the data access that run the procedure
                //by procedure name, and params
                var t = DataAccess.ExecuteStoredProcedure<Products>("GetAllProducts", langParam);
                //the option to run it...
                return t.ToList();
            }
            catch (Exception ex)            
            {
                //write to logger
                throw new Exception("");
            }
        }

        //func to get the review by prod id
        public List<Products> GetProductsByCategory(int categoriId, int lang)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter categoriIdParam = new SqlParameter("@cat", categoriId);
                SqlParameter langParam = new SqlParameter("@Lang", lang);

                SqlParameter[] parameters = new[] { categoriIdParam, langParam };
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Products>("GetReviewsByProdId", parameters);
                return t.ToList();
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

        //is this func delete H & E products?????????
        public bool DeleteProductsCategory(int productId, int cat)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter productIdParam = new SqlParameter("@productId", productId);
                SqlParameter catParam = new SqlParameter("@cat", cat);

                SqlParameter[] parameters = new[] { productIdParam, catParam };
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Products>("DeleteProductsCategory", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

 
        // public bool PostProduct(Product p, string nameE, string descE)
        public bool PostProduct(string NameH, string DescriptionH, string NameE, string DescriptionE, decimal Price, string ImageURL, decimal SalePrice)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter NameHParam = new SqlParameter("@NameH", NameH);
                SqlParameter DescriptionHParam = new SqlParameter("@DescriptionH", DescriptionH);
                SqlParameter NameEParam = new SqlParameter("@NameE", NameE);
                SqlParameter DescriptionEParam = new SqlParameter("@DescriptionE", DescriptionE);
                SqlParameter PriceParam = new SqlParameter("@Price", Price);
                SqlParameter ImageURLParam = new SqlParameter("@ImageURL", ImageURL);
                SqlParameter SalePriceParam = new SqlParameter("@SalePrice", SalePrice);

                SqlParameter[] parameters = new[] { SalePriceParam, ImageURLParam, PriceParam, DescriptionEParam, NameEParam, DescriptionHParam, NameHParam, };
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Products>("PostProduct", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }


        public bool PutProduct(int id, string NameH, string DescriptionH, string NameE, string DescriptionE, decimal Price, string ImageURL, decimal SalePrice)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter idParam = new SqlParameter("@id", id);
                SqlParameter NameHParam = new SqlParameter("@NameH", NameH);
                SqlParameter DescriptionHParam = new SqlParameter("@DescriptionH", DescriptionH);
                SqlParameter NameEParam = new SqlParameter("@NameE", NameE);
                SqlParameter DescriptionEParam = new SqlParameter("@DescriptionE", DescriptionE);
                SqlParameter PriceParam = new SqlParameter("@Price", Price);
                SqlParameter ImageURLParam = new SqlParameter("@ImageURL", ImageURL);
                SqlParameter SalePriceParam = new SqlParameter("@SalePrice", SalePrice);

                SqlParameter[] parameters = new[] { SalePriceParam, ImageURLParam, PriceParam, DescriptionEParam, NameEParam, DescriptionHParam, NameHParam, idParam };
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Products>("PutProduct", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }


    }
}
