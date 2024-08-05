﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using Npgsql;

namespace DesigneryCore.Services
{
    public class CategoriesService : ICategoriesService
    {

        public List<Categories> GetAllCategories()
        {
            try
            {

                var t = DataAccessPostgreSQL.ExecuteStoredProcedureWithCursor<Categories>("GetAllCategories", null);
                //var t = DataAccessSQL.ExecuteStoredProcedure<Categories>("GetAllCategories", null);
                return t.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public Categories GetCategoryById(int id) {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                 new SqlParameter("@CategoryId", id)
                };
                var t = DataAccessSQL.ExecuteStoredProcedure<Categories>("GetCategoryById", param);
                return t.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Err by get data from server");
            }
        }
        public bool postCategories(Categories c)
        {


            try
            { // בדיקה אם יש תמונה למוצר
                if (c.Image != null)
                {
                    // קביעת התיקיה שבה נשמור את התמונות
                    var uploadsDir = Path.Combine("wwwroot", "images");
                    // בדיקה אם התיקיה קיימת, אם לא - יצירת התיקיה
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    // יצירת שם קובץ ייחודי עם GUID + שם הקובץ המקורי
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(c.Image.FileName);
                    var filePath = Path.Combine(uploadsDir, uniqueFileName);

                    // פתיחת קובץ לשמירת התמונה
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        // העתקת התמונה לזרם הקובץ
                        c.Image.CopyTo(stream);
                    }

                    // שמירת הנתיב של התמונה במשתנה ImageURL של המוצר
                    c.ImageURL = $"/images/{uniqueFileName}";
                }

                List<SqlParameter> listParm = new List<SqlParameter>()
                {
                 new SqlParameter("@NameH", c.NameHe),
                 new SqlParameter("@DescriptionH", c.DescriptionHe),
                 new SqlParameter("@NameE", c.NameEn),
                 new SqlParameter("@DescriptionE", c.DescriptionEn),
                 new SqlParameter("@Upcategory", c.UpCategory),
                 new SqlParameter("@ImageURL", c.ImageURL)

                };
                var r = DataAccessSQL.ExecuteStoredProcedure<Categories>("PostCategory", listParm);
                return true;
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }
        public bool PutCategories(int cId, Categories c)
        {
            try
            {
                List<NpgsqlParameter> listParm = new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("@p_id", cId),
            new NpgsqlParameter("@p_nameh", c.NameHe),
            new NpgsqlParameter("@p_descriptionh", c.DescriptionHe),
            new NpgsqlParameter("@p_namee", c.NameEn),
            new NpgsqlParameter("@p_descriptione", c.DescriptionEn),
            new NpgsqlParameter("@p_upcategory", c.UpCategory),
            new NpgsqlParameter("@p_imageurl", c.ImageURL)
        };

                var result = DataAccessPostgreSQL.ExecuteFunction("PutCategory", listParm);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }

        public List<Categories> GetUpCategoriesByCategoryID(int cId)
        {
            try
            {
                List<SqlParameter> @ParentCategories = new List<SqlParameter>()
                {
                    new SqlParameter("@CategoryID", cId),
                 };
                var r = DataAccessSQL.ExecuteStoredProcedure<Categories>("GetUpCategoriesByCategoryID", @ParentCategories);
                return r.ToList();
            }
            catch (Exception)
            {
                throw new Exception();
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
    }
}
