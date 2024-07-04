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
    public class UserService : IUserService
    {

        //to check if is static function//
        //to check if the return type has to be IEnumerable or not//
        public List<User> GetAllUsers()
        {
            try
            {
                var t = DataAccess.ExecuteStoredProcedure<User>("GetAllUsers", null);
                return t.ToList();
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("hello");
            }
        }

        public User Login(string email, string password)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter("@mail", email);
                SqlParameter parm2 = new SqlParameter("@pas", password);
             
                var u =DataAccess.ExecuteStoredProcedure<User>("Login", [parm1, parm2]);
                return (User)u.ToList()[0];
            }
            catch (Exception ex) 
            {
                throw new Exception("hello"); 
            }
        }

        public bool PostUser(User user)
        {
            try
            {
                List<SqlParameter> listParm = new List<SqlParameter>()
                {
                 new SqlParameter("@Name",user.Name ),
                 new SqlParameter("@Email",user.Email ),
                 new SqlParameter("@PhoneNumber", user.PhoneNumber),
                 new SqlParameter("@PasswordHash", user.PasswordHash),
                 new SqlParameter("@TypeID", user.TypeID),
                 new SqlParameter("@Credits", user.Credits) 
                };
                var u = DataAccess.ExecuteStoredProcedure<User>("PostUser", listParm);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("hello");
            }
        }

        public bool PutUser(int id, User user)
        {
            try
            {
                List<SqlParameter> listParm = new List<SqlParameter>()
                {
                 new SqlParameter("@id",id),
                 new SqlParameter("@Name",user.Name ),
                 new SqlParameter("@Email",user.Email ),
                 new SqlParameter("@PhoneNumber", user.PhoneNumber),
                 new SqlParameter("@PasswordHash", user.PasswordHash),
                 new SqlParameter("@TypeID", user.TypeID),
                 new SqlParameter("@Credits", user.Credits)
                };
                var u = DataAccess.ExecuteStoredProcedure<User>("PutUser", listParm);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("hello");
            }
        }
    }
}


//public static IEnumerable<Course> GetAllCourses()
//{
//    Course course = new Course();
//    var t = DataAccess<Course>.ExecuteStoredProcedure("getAllCourses", null);
//    return t;
//}
