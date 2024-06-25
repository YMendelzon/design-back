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
        public DateTime CreatedAt { get; set; }
        public int TypeID { get; set; }
        public int? Credits { get; set; }


        public static User MapUser(IDataReader reader)
        {
            if (reader.Read())
            {
                User user = new User();

                user.UserID = Convert.ToInt32(reader["UserID"]);
                user.Name = reader["Name"].ToString();
                user.Email = reader["Email"].ToString();
                user.PhoneNumber = reader["PhoneNumber"].ToString();
                user.PasswordHash = reader["PasswordHash"].ToString();
                user.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                user.TypeID = Convert.ToInt32(reader["TypeID"]);
                user.Credits = reader["Credits"] != DBNull.Value ? Convert.ToInt32(reader["Credits"]) : (int?)null;

                //user.Credits = Convert.ToInt32(reader["Credits"]);

                return user;
            }

            return null;
        }
    }
}
