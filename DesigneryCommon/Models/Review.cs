using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int? ProductID { get; set; }
        public int? UserID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }


        //function to map the value!
        //public static Review MapReview(IDataReader reader)
        //{
        //    if (reader.Read())
        //    {
        //        Review review = new Review();

        //        review.ReviewID = Convert.ToInt32(reader["ReviewID"]);
        //        review.ProductID = reader["ProductID"] != DBNull.Value ? Convert.ToInt32(reader["ProductID"]) : (int?)null;
        //        review.UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : (int?)null;
        //        review.Rating = Convert.ToInt32(reader["Rating"]);
        //        review.Comment = reader["Comment"] != DBNull.Value ? (reader["Comment"]).ToString() : (string?)null;
        //        review.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

        //        return review;
        //    }

        //    return null;
        //}
    }
}
