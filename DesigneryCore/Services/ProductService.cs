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

        public List<Products> GetAllProducts()
        {
            try
            {
                //called the function from the data access that run the procedure
                //by procedure name, and params
                
                
                var t = DataAccess.ExecuteStoredProcedure<Products>("GetAllProducts",null);
                var x = t;
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
        public List<Products> GetProductsByCategory(int categoriId)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter categoriIdParam = new SqlParameter("@cat", categoriId);

                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Products>("GetReviewsByProdId", [categoriIdParam] );
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
                List<SqlParameter> productIdParam = new List<SqlParameter>() {
                    new SqlParameter("@productId", productId),
                    new SqlParameter("@cat", cat)
            };
                //SqlParameter[] parameters = new[] { productIdParam, catParam };
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Products>("DeleteProductsCategory", productIdParam);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }


        // public bool PostProduct(Product p, string nameE, string descE)
        public bool PostProduct(Products p)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new List<SqlParameter>() {
               new SqlParameter("@NameHe", p.NameHe),
               new SqlParameter("@DescriptionHe", p.DescriptionHe),
               new SqlParameter("@NameEn", p.NameEn),
               new SqlParameter("@DescriptionEn", p.DescriptionEN),
               new SqlParameter("@Price", p.Price),
               new SqlParameter("@ImageURL", p.ImageURL),
               new SqlParameter("@SalePrice", p.SalePrice)
            };

                //SqlParameter[] parameters = new[] { SalePriceParam, ImageURLParam, PriceParam, DescriptionEParam, NameEParam, DescriptionHParam, NameHParam };
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


        public bool PutProduct(int id, Products p)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new List<SqlParameter>() {

               new SqlParameter("@id", id),
               new SqlParameter("@NameHe", p.NameHe),
               new SqlParameter("@DescriptionHe", p.DescriptionHe),
               new SqlParameter("@NameEn", p.NameEn),
               new SqlParameter("@DescriptionEn", p.DescriptionEN),
               new SqlParameter("@Price", p.Price),
               new SqlParameter("@ImageURL", p.ImageURL),
               new SqlParameter("@SalePrice", p.SalePrice)
            };
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
