using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? CreateAt { get; set; }
        public int TypeID { get; set; }
        public int? Credits { get; set; }
    }


    //public User MapUser(IDataReader reader)
    //{
    //    if (reader.Read())
    //    {
    //        User course = new User();

    //        course.courses_id = Convert.ToInt32(reader["courses_id"]);
    //        course.courses_name = reader["courses_name"].ToString();
    //        course.title = reader["title"].ToString();
    //        course.description = reader["description"].ToString();
    //        course.Price = Convert.ToInt32(reader["Price"]);
    //        //Recommendations = new List<Recommendation>();

    //        return course;
    //    }
    //    return null;
    //}
}
