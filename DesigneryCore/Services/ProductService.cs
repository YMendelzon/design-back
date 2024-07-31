using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
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
                var t = DataAccessSQL.ExecuteStoredProcedure<Product>("GetAllProducts", null);
                //the option to run it...
                return t.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("");
            }
        }
        public bool PostProduct(Product product)
        {
            try
            {
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
                throw new Exception("שגיאה בהוספת מוצר: " + ex.Message);
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
        public List<Product> GetProductsByCategory(int categoriId)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter categoriIdParam = new SqlParameter("@cat", categoriId);

                //send to the function the param
                var t = DataAccessSQL.ExecuteStoredProcedure<Product>("GetProductsByCategory", [categoriIdParam]);

                return t.ToList();
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
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
                //send to the function the param[                   DeleteProductsCategory
                var t = DataAccessSQL.ExecuteStoredProcedure<Product>("DeleteProductsCategory", productIdParam);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

        public List<Product> GetProducts()
        {
            var t = DataAccess.ExecuteStoredProcedure<Product>("GetAllProducts", null);
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

                var imageUrlResult = DataAccess.ExecuteStoredProcedure<Product>("GetProductImageURL", getImageUrlParams);
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

                DataAccess.ExecuteStoredProcedure<Product>("DeleteProduct", deleteProductParams);

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
            List<SqlParameter> getImageUrlParams = new List<SqlParameter> {
            new SqlParameter("@ProductID", p.ProductID)
        };
            var imageUrlResult = DataAccess.ExecuteStoredProcedure<Product>("GetProductImageURL", getImageUrlParams);
            string imageUrl = imageUrlResult.FirstOrDefault()?.ImageURL;
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new Exception("לא נמצא URL של התמונה עבור המוצר");
            }
            _s3Service.DeleteFileAsync(imageUrl).Wait();
            if (p.Image != null)
            {
                var imageUrll = await _s3Service.UploadFileAsync(p.Image);
                p.ImageURL = imageUrll;
            }

            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter("@id",p.ProductID),
         new SqlParameter("@NameHe", p.NameHe),
         new SqlParameter("@DescriptionHe", p.DescriptionHe),
         new SqlParameter("@NameEn", p.NameEn),
         new SqlParameter("@DescriptionEn", p.DescriptionEn),
         new SqlParameter("@Price", p.Price),
         new SqlParameter("@ImageURL", p.ImageURL),
         new SqlParameter("@SalePrice", p.SalePrice),
         //new SqlParameter("@IsRecommended", p.IsRecommended)
     };
             DataAccess.ExecuteStoredProcedure<Product>("PutProduct", parameters);
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
            catch(Exception err)
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
            catch(Exception er)
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

