﻿using DesigneryCommon.Models;
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

        public List<Product> GetAllProduct()
        {
            try
            {
                //called the function from the data access that run the procedure
                //by procedure name, and params
                var t = DataAccess.ExecuteStoredProcedure<Product>("GetAllProducts",null);
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
        public List<Product> GetProductByCategory(int categoriId)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter categoriIdParam = new SqlParameter("@cat", categoriId);

                //send to the function the param

                var t = DataAccess.ExecuteStoredProcedure<Products>("GetProductsByCategory", [categoriIdParam] );

                return t.ToList();
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

        //is this func delete H & E Product?????????
        public bool DeleteProductCategory(int productId, int cat)
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
                var t = DataAccess.ExecuteStoredProcedure<Product>("DeleteProductCategory", productIdParam);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }


        // public bool PostProduct(Product p, string nameE, string descE)
        public bool PostProduct(Product p)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new List<SqlParameter>() {

               new SqlParameter("@NameHe", p.NameHe),
               new SqlParameter("@DescriptionHe", p.DescriptionHe),
               new SqlParameter("@NameEn", p.NameEn),
               new SqlParameter("@DescriptionEn", p.DescriptionEn),
               new SqlParameter("@Price", p.Price),
               new SqlParameter("@ImageURL", p.ImageURL),
               new SqlParameter("@SalePrice", p.SalePrice)
            };

                //SqlParameter[] parameters = new[] { SalePriceParam, ImageURLParam, PriceParam, DescriptionEParam, NameEParam, DescriptionHParam, NameHParam };
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Product>("PostProduct", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
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
               new SqlParameter("@SalePrice", p.SalePrice)
            };
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Product>("PutProduct", parameters);
                return true;
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
                var t = DataAccess.ExecuteStoredProcedure<Products>("PostProductsCategory", parameters);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
