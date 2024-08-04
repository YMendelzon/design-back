using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Services
{
    public class ReviewService : IReviewService
    {
        //function to get all reviews
        public List<Review> GetAllReviews()
        {
            try
            {
                //called the function from the data access that run the procedure
                //by procedure name, and params
                var t = DataAccessPostgreSQL.ExecuteFunction<Review>("GetAllReviews", null);
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
        public List<Review> GetReviewsByProdId(int prodId)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@prodId", prodId)
                };

                var t = DataAccessSQL.ExecuteStoredProcedure<Review>("GetReviewsByProdId", param);
                return t.ToList();
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

        //function to add review - the userId running this function
        public bool PostReview(Review review)
        {
            try
            {
                // יצירת הפרמטרים עבור הפונקציה
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("@p0", review.ProductID),
            new NpgsqlParameter("@p1", review.UserID),
            new NpgsqlParameter("@p2", review.Rating),
            new NpgsqlParameter("@p3", review.Comment)
        };

                // שליחת הפונקציה עם הפרמטרים ל-DataAccessPostgreSQL
                DataAccessPostgreSQL.ExecuteFunction("postreviews", parameters);

                // בהנחה שהפונקציה לא מחזירה ערך, תמיד נחזיר true
                return true;
            }
            catch (Exception ex)
            {
                // כתיבה ללוג במקרה של שגיאה
                throw new Exception("Error in PostReview", ex);
            }
        }

    }
}

