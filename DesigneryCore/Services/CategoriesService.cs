using System;
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
        private readonly S3Service _s3Service;

        public CategoriesService(S3Service s3Service)
        {
            _s3Service = s3Service;
        }
        public List<Categories> GetAllCategories()
        {
            try
            {
                var t = DataAccessPostgreSQL.ExecuteFunction<Categories>("GetAllCategoriesF", null);
                return t.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Categories GetCategoryById(int id)
        {
            try
            {
                List<NpgsqlParameter> param = new()
                {
                 new ("p_CategoryId", id)
                };
                var t = DataAccessPostgreSQL.ExecuteFunction<Categories>("GetCategoryByIdF", param);
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
            {
                if (c.Image != null)
                {
                    var uploadsDir = Path.Combine("wwwroot", "images");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(c.Image.FileName);
                    var filePath = Path.Combine(uploadsDir, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        c.Image.CopyTo(stream);
                    }
                    c.ImageURL = $"/images/{uniqueFileName}";
                }

                List<NpgsqlParameter> listParm = new()
                {
                 new ("p_nameh", c.NameHe),
                 new ("p_descriptionh", c.DescriptionHe),
                 new ("p_namee", c.NameEn),
                 new ("p_descriptione", c.DescriptionEn),
                 new ("p_upcategory", c.UpCategory),
                 new ("p_imageurl", c.ImageURL)

                };
                var r = DataAccessPostgreSQL.ExecuteFunction<Categories>("PostCategory", listParm);
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

        //public List<Categories> GetUpCategoriesByCategoryID(int cId)
        //{
        //    try
        //    {
        //        List<SqlParameter> @ParentCategories = new List<SqlParameter>()
        //        {
        //            new SqlParameter("@CategoryID", cId),
        //         };
        //        var r = DataAccessSQL.ExecuteStoredProcedure<Categories>("GetUpCategoriesByCategoryID", @ParentCategories);
        //        return r.ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception();
        //    }
        //}
        //לא מעודכן בDB עדיין
        public List<Categories> GetSubcategories(int categoryId)
        {
            try
            {
                NpgsqlParameter categoryIdParamter = new NpgsqlParameter("@CategoryId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = categoryId };
                var t = DataAccessPostgreSQL.ExecuteFunction<Categories>("get_subcategories", [categoryIdParamter]);
                return t.ToList();
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
    }
}
