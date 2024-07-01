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
                //write to logger
                throw new Exception("Hello");
            }

        }

        public bool postCategories(Categories c)
        {
            try
            {
                List<SqlParameter> listParm = new List<SqlParameter>()
                {
                 new SqlParameter("@NameH", c.NameHe),
                 new SqlParameter("@DescriptionH", c.DescriptionHe),
                 new SqlParameter("@NameE", c.NameEn),
                 new SqlParameter("@DescriptionE", c.DescriptionEn),
                 new SqlParameter("@Upcategory", c.UpCategory)

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
