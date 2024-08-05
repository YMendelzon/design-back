using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;
using static iText.IO.Image.Jpeg2000ImageData;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DesigneryCore.Services
{
    public class ProductService : IProductService
    {
        private readonly S3Service _s3Service;

        public ProductService(S3Service s3Service)
        {
            _s3Service = s3Service;
        }
        //public List<Product> GetAllProducts()
        //{
        //    try
        //    {
        //        //called the function from the data access that run the procedure
        //        //by procedure name, and params
        //        var t = DataAccess.ExecuteStoredProcedure<Product>("GetAllProducts", null);
        //        //the option to run it...
        //        return t.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        //write to logger
        //        throw new Exception("");
        //    }
        //}
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
                throw new Exception("");
            }
        }
        // check why itws didnt work?????????

        public bool PostProduct(Product product)
        {
            try
            {
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("@p_namehe", product.NameHe),
            new NpgsqlParameter("@p_descriptionhe", product.DescriptionHe),
            new NpgsqlParameter("@p_nameen", product.NameEn),
            new NpgsqlParameter("@p_descriptionen", product.DescriptionEn),
            new NpgsqlParameter("@p_price", product.Price),
            new NpgsqlParameter("@p_imageurl", product.ImageURL),
            new NpgsqlParameter("@p_saleprice", product.SalePrice),
            new NpgsqlParameter("@p_isrecommended", product.IsRecommended)
        };

                var result = DataAccessPostgreSQL.ExecuteFunction("PostProduct", parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("שגיאה בהוספת מוצר: " + ex.Message, ex);
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
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("@productid", proId),
            new NpgsqlParameter("@cat", catId)
        };

                var result = DataAccessPostgreSQL.ExecuteFunction("PostProductsCategory", parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while posting the product category.", ex);
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

        public List<Product> GetProducts()
        {
            var t = DataAccessSQL.ExecuteStoredProcedure<Product>("GetAllProducts", null);
            return t.ToList();
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                // Step 1: Retrieve the product's image URL from the database using a stored procedure
                List<SqlParameter> getImageUrlParams = new List<SqlParameter> {
            new SqlParameter("@ProductID", productId)
        };

                var imageUrlResult = DataAccessSQL.ExecuteStoredProcedure<Product>("GetProductImageURL", getImageUrlParams);
                if (imageUrlResult == null || imageUrlResult.Count() == 0)
                {
                    throw new Exception("מוצר לא נמצא");
                }

                string imageUrl = imageUrlResult.FirstOrDefault()?.ImageURL;
                if (string.IsNullOrEmpty(imageUrl))
                {
                    throw new Exception("לא נמצא URL של התמונה עבור המוצר");
                }

                // Step 2: Delete the image from S3
                _s3Service.DeleteFileAsync(imageUrl).Wait();

                // Step 3: Delete the product from the database
                List<SqlParameter> deleteProductParams = new List<SqlParameter> {
            new SqlParameter("@ProductID", productId)
        };

                DataAccessSQL.ExecuteStoredProcedure<Product>("DeleteProduct", deleteProductParams);

                return true;
            }
            catch (Exception ex)
            {
                // Debug: Print the exception message
                Console.WriteLine("Exception: " + ex.Message);
                throw new Exception("שגיאה במחיקת מוצר: " + ex.Message);
            }
        }

        public async Task<bool> PutProduct(Product p)
        {
            // השגת URL של התמונה הקיימת
            List<NpgsqlParameter> getImageUrlParams = new List<NpgsqlParameter> {
        new NpgsqlParameter("@p0", p.ProductID)
            };

            //var imageUrlResult= DataAccessPostgreSQL.ExecuteFunction<string>("GetProductImageURL", getImageUrlParams);
            //string imageUrl = imageUrlResult.FirstOrDefault();

            //if (string.IsNullOrEmpty(imageUrl))
            //{
            //    throw new Exception("לא נמצא URL של התמונה עבור המוצר");
            //}

            // מחיקת התמונה הקיימת
           // _s3Service.DeleteFileAsync(imageUrl).Wait();

            // העלאת התמונה החדשה
            if (p.Image != null)
            {
                var imageUrll = await _s3Service.UploadFileAsync(p.Image);
                p.ImageURL = imageUrll;
            }

            // יצירת הפרמטרים עבור הפונקציה
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
    {
        new NpgsqlParameter("@p0", p.ProductID),
        new NpgsqlParameter("@p1", p.NameHe),
        new NpgsqlParameter("@p2", p.DescriptionHe),
        new NpgsqlParameter("@p3", p.NameEn),
        new NpgsqlParameter("@p4", p.DescriptionEn),
        new NpgsqlParameter("@p5", p.Price),
        new NpgsqlParameter("@p6", p.ImageURL),
        new NpgsqlParameter("@p7", p.SalePrice),
        new NpgsqlParameter("@p8", p.IsRecommended)
    };

            // שליחת הפונקציה עם הפרמטרים ל-DataAccessPostgreSQL
            DataAccessPostgreSQL.ExecuteFunction("PutProduct", parameters);
            return true;
        }

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

