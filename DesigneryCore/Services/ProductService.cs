using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
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

        public List<Product> GetAllProducts()
        {
            try
            {
                //called the function from the data access that run the procedure
                //by procedure name, and params
                var t = DataAccessPostgreSQL.ExecuteFunction<Product>("GetAllProducts", null);
                //the option to run it...
                return t;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }
        public bool PostProduct(Product product)
        {
            try
            { // בדיקה אם יש תמונה למוצר
              //if (product.ImageURL != null)
              //{
              //    // קביעת התיקיה שבה נשמור את התמונות
              //    var uploadsDir = Path.Combine("wwwroot", "images");
              //    // בדיקה אם התיקיה קיימת, אם לא - יצירת התיקיה
              //    if (!Directory.Exists(uploadsDir))
              //    {
              //        Directory.CreateDirectory(uploadsDir);
              //    }
              //    // יצירת שם קובץ ייחודי עם GUID + שם הקובץ המקורי
              //    //var uniqueFileName = Path.GetFileName(product.ImageURL);
              //    var filePath = Path.Combine(uploadsDir, product.ImageURL);
              //    //// פתיחת קובץ לשמירת התמונה
              //    //using (var stream = new FileStream(filePath, FileMode.Create))
              //    //{
              //    //    // העתקת התמונה לזרם הקובץ
              //    //    product.ImageURL.CopyTo(stream);
              //    //}
              //    // שמירת הנתיב של התמונה במשתנה ImageURL של המוצר
              //   // product.ImageURL = $"/images/{ product.ImageURL}";
              //}

                List<SqlParameter> parameters = new List<SqlParameter>() {
                   new SqlParameter("@NameHe", product.NameHe),
                   new SqlParameter("@DescriptionHe", product.DescriptionHe),
                   new SqlParameter("@NameEn", product.NameEn),
                   new SqlParameter("@DescriptionEn", product.DescriptionEn),
                   new SqlParameter("@Price", product.Price),
                   new SqlParameter("@ImageURL", product.ImageURL),
                   new SqlParameter("@SalePrice", product.SalePrice),
                   new SqlParameter("@IsRecommended", product.IsRecommended)
                };

                //send to the function the param
                var t = DataAccessSQL.ExecuteStoredProcedure<Product>("PostProduct", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("err");
            }
        }

        public bool PutProduct(int id, Product p)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new List<SqlParameter>() {
               new SqlParameter("@id", id),
               new SqlParameter("@NameHe", p.NameHe),
               new SqlParameter("@DescriptionHe", p.DescriptionHe),
               new SqlParameter("@NameEn", p.NameEn),
               new SqlParameter("@DescriptionEn", p.DescriptionEn),
               new SqlParameter("@Price", p.Price),
               new SqlParameter("@ImageURL", p.ImageURL),
               new SqlParameter("@SalePrice", p.SalePrice),
               new SqlParameter("@IsRecommended", p.IsRecommended)
            };
                //send to the function the param
                var t = DataAccessSQL.ExecuteStoredProcedure<Product>("PutProduct", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

        //func to get the review by prod id
        public List<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("p_cat", NpgsqlTypes.NpgsqlDbType.Integer) { Value = categoryId }
        };

                return DataAccessPostgreSQL.ExecuteFunction<Product>("GetProductsByCategory", parameters);
            }
            catch (Exception ex)
            {
                // Log detailed error message
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving products by category.", ex);
            }
        }


        public bool PostProductCategory(int proId, int catId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                     new SqlParameter("@productId", proId),
                     new SqlParameter("@cat", catId)
                };
                var t = DataAccessSQL.ExecuteStoredProcedure<Product>("PostProductsCategory", parameters);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


        //is this func delete H & E Product?????????
        //public bool DeleteProductsCategory(int productId, int cat)
        //{
        //    try
        //    {
        //        // יצירת הפרמטר עבור stored procedure
        //        List<SqlParameter> productIdParam = new List<SqlParameter>() {
        //            new SqlParameter("@productId", productId),
        //            new SqlParameter("@cat", cat)
        //    };
        //        //SqlParameter[] parameters = new[] { productIdParam, catParam };
        //        //send to the function the param[                   DeleteProductsCategory
        //        var t = DataAccessSQL.ExecuteStoredProcedure<Product>("DeleteProductsCategory", productIdParam);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //write to logger
        //        throw new Exception("");
        //    }
        //}


        public bool DeleteProductsCategory(int productId, int cat)
        {
            try
            {
                // יצירת הפרמטרים עבור stored procedure
                var p1 = new NpgsqlParameter("p_productId", NpgsqlDbType.Integer);
                p1.Value = productId;
                var p2 = new NpgsqlParameter("p_cat", NpgsqlDbType.Integer);
                p2.Value = cat;

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter> { p1, p2 };

                // קריאה לפונקציה שמבצעת את ה-stored procedure
                DataAccessPostgreSQL.ExecuteStoredProcedure("DeleteProductsCategory", parameters);
                return true;
            }
            catch (Exception ex)
            {
                // רישום שגיאה ליומן
                throw new Exception("Error executing stored procedure: " + ex.Message);
            }
        }


        //public List<Product> GetRecommendedProducts()
        //{
        //    try
        //    {
        //        var q = DataAccessSQL.ExecuteStoredProcedure<Product>("GetRecommendedProducts", null);
        //        return q.ToList();
        //    }
        //    catch
        //    {
        //        throw new Exception();
        //    }
        //}

        public List<Categories> GetCategoriesHierarchyByProductId(int productId)
        {
            try
            {
                List<SqlParameter> productIdParam = new List<SqlParameter>()
                {
                    new SqlParameter("@ProductId", productId)
                };
                var t = DataAccessSQL.ExecuteStoredProcedure<Categories>("CategoriesHierarchyByProductId", productIdParam);
                return t.ToList();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }


        public List<Categories> GetSubcategories(int categoryId)
        {
            try
            {
                List<SqlParameter> productIdParam = new List<SqlParameter>()
                {
                    new SqlParameter("@ParentCategoryID", categoryId)
                };
                var t = DataAccessSQL.ExecuteStoredProcedure<Categories>("GetSubcategories", productIdParam);
                return t.ToList();
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }

        public List<Product> GetProductsByCategoryAndSubcategories(int categoryId)
        {
            try
            {
                List<SqlParameter> productIdParam = new List<SqlParameter>()
                {
                    new SqlParameter("@CategoryID", categoryId)
                };
                var t = DataAccessSQL.ExecuteStoredProcedure<Product>("GetProductsByCategoryAndSubcategories", productIdParam);
                return t.ToList();
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
    }
}
