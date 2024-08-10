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
using iText.Layout.Element;
using System.Collections;
namespace DesigneryCore.Services
{
    public class ProductService : IProductService
    {
        private readonly S3Service _s3Service;

        public ProductService(S3Service s3Service)
        {
            _s3Service = s3Service;
        }
        public List<Product> GetAllProducts()
        {
            try
            {
                var t = DataAccessPostgreSQL.ExecuteFunction<Product>("GetAllProducts", null);
                return t;
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
                List<NpgsqlParameter> parameters = new()
             {
                new ("p_namehe", product.NameHe),
                new ("p_descriptionhe", product.DescriptionHe),
                new ("p_nameen", product.NameEn),
                new ("p_descriptionen", product.DescriptionEn),
                new ("p_price", product.Price),
                new ("p_imageurl", product.ImageURL),
                new ("p_saleprice", product.SalePrice),
                new ("p_isrecommended", product.IsRecommended)
            };

                var result = DataAccessPostgreSQL.ExecuteFunction("PostProduct", parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("שגיאה בהוספת מוצר: " + ex.Message, ex);
            }
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                List<NpgsqlParameter> parameters = new()
                {
                 new ("p_cat", categoryId )
                };

                return DataAccessPostgreSQL.ExecuteFunction<Product>("GetProductsByCategory", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving products by category.", ex);
            }
        }


        public bool PostProductCategory(int proId, int catId)
        {
            try
            {
                List<NpgsqlParameter> parameters = new()
        {
            new ("@productid", proId),
            new ("@cat", catId)
        };

                var result = DataAccessPostgreSQL.ExecuteFunction("PostProductsCategory", parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while posting the product category.", ex);
            }
        }

        public bool DeleteProductsCategory(int productId, int cat)
        {
            try
            {
                List<NpgsqlParameter> parameters = new() {
                    new("p_idProduct", productId),
                    new("p_idCategory",cat )
                };

                DataAccessPostgreSQL.ExecuteFunction("deleteProductCategory", parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing stored procedure: " + ex.Message);
            }
        }

        public async Task<bool> PutProduct(Product p)
        {
            // השגת URL של התמונה הקיימת
            List<NpgsqlParameter> getImageUrlParams = new List<NpgsqlParameter> {
                  new NpgsqlParameter("p_id", p.ProductID)
            };

            var imageUrlResult = DataAccessPostgreSQL.ExecuteFunction<List<string>>("GetProductImageURL", getImageUrlParams);

            if (p.Image != null)
            {
                var imageUrll = await _s3Service.UploadFileAsync(p.Image);
                p.ImageURL = imageUrll;
            }

            List<NpgsqlParameter> parameters = new()
            {
                new ("@p0", p.ProductID),
                new ("@p1", p.NameHe),
                new ("@p2", p.DescriptionHe),
                new ("@p3", p.NameEn),
                new ("@p4", p.DescriptionEn),
                new ("@p5", p.Price),
                new ("@p6", p.ImageURL),
                new ("@p7", p.SalePrice),
                new ("@p8", p.IsRecommended)
            };

            DataAccessPostgreSQL.ExecuteFunction("PutProduct", parameters);
            return true;
        }

        public List<Categories> GetCategoriesHierarchyByProductId(int productId)
        {
            try
            {
                var productIdParam = new List<NpgsqlParameter>
         {
             new NpgsqlParameter("@p0", NpgsqlTypes.NpgsqlDbType.Integer) { Value = productId }
         };
                var t = DataAccessPostgreSQL.ExecuteFunction<Categories>("categories_hierarchy_by_product_id", productIdParam);
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
                NpgsqlParameter categoryIdParamter = new NpgsqlParameter("@CategoryId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = categoryId };
                var t = DataAccessPostgreSQL.ExecuteFunction<Product>("get_products_by_category_and_subcategories", [categoryIdParamter]);
                return t.ToList();
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }

        }
    }

}