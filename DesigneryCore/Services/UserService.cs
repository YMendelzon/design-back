using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using System;
using System.Collections.Generic;
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
    }
}


//public static IEnumerable<Course> GetAllCourses()
//{
//    Course course = new Course();
//    var t = DataAccess<Course>.ExecuteStoredProcedure("getAllCourses", null);
//    return t;
//}
