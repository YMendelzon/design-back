﻿using DesigneryCommon.Models;
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
        public List<Review> GetReviewsByProdId(int prodId)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@prodId", prodId)
                }; 
                
                var t = DataAccess.ExecuteStoredProcedure<Review>("GetReviewsByProdId", param);
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
                // יצירת הפרמטר עבור stored procedure
                // יצירת פרמטרים
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                     //new SqlParameter("@id", cqId),
                    new SqlParameter("@ProductID", review.ProductID),
                    new SqlParameter("@UserID", review.UserID),
                    new SqlParameter("@Rating", review.Rating),
                    new SqlParameter("@Comment", review.Comment)
                };
                // הוספת הפרמטרים למערך
                var t = DataAccess.ExecuteStoredProcedure<Review>("PostReviews", parameters);

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

