using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;

namespace DesigneryCore.Services
{
    public class CategoriesService : ICategoriesService
    {
       
        public List<Categories> GetAllCategories()
        {
            try
            {
                var t = DataAccess.ExecuteStoredProcedure<Categories>("GetAllCategories", null);
                return t.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Hello");
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
                var r = DataAccess.ExecuteStoredProcedure<Categories>("PostCategory", listParm);
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
                List<SqlParameter> listParm = new List<SqlParameter>()
                {
                    new SqlParameter("@id", cId),
                    new SqlParameter("@NameH", c.NameHe),
                    new SqlParameter("@DescriptionH", c.DescriptionHe),
                    new SqlParameter("@NameE", c.NameEn),
                    new SqlParameter("@DescriptionE", c.DescriptionEn),
                    new SqlParameter("@ImageURL", c.ImageURL)
                 };
                var r = DataAccess.ExecuteStoredProcedure<Categories>("putCategory", listParm);
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
