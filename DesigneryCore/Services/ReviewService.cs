using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
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
                var t = DataAccess.ExecuteStoredProcedure<Review>("GetAllReviews", null);
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
        public Review GetReviewsByProdId(int prodId)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                SqlParameter prodIdParam = new SqlParameter("@prodId", prodId);
                //send to the function the param
                var t = DataAccess.ExecuteStoredProcedure<Review>("GetReviewsByProdId", prodIdParam);
                return t.FirstOrDefault();
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

        //function to add review - the userId running this function
        public bool PostReview(int productID, int userId, int rating, string comment)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                // יצירת פרמטרים
                SqlParameter prodIdParam = new ("@ProductID", productID);
                SqlParameter userIdParam = new ("@UserID", userId);
                SqlParameter ratingParam = new ("@Rating", rating);
                SqlParameter commentParam = new ("@Comment", comment);

                // הוספת הפרמטרים למערך
                SqlParameter[] parameters = new [] { prodIdParam, userIdParam, ratingParam, commentParam };
                var t = DataAccess.ExecuteStoredProcedure<bool>("PostReviews", parameters);

                //to check if this the return value
                return t.Any();
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }
    }
}
