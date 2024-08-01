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
    public class UserService : IUserService
    {
        private readonly RefreshTokenStore _refreshTokenStore;

        public UserService(RefreshTokenStore refreshTokenStore)
        {
            _refreshTokenStore = refreshTokenStore;
        }
        //to check if is static function//
        //to check if the return type has to be IEnumerable or not//
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            try
            {
                // קריאה לפונקציה שמבצעת את ה-SELECT על הפונקציה GetAllUsers
                users = DataAccessPostgreSQL.ExecuteFunction<User>("GetAllUsers");
            }
            catch (Exception ex)
            {
                // רישום שגיאה ליומן
                throw new Exception("Error executing function: " + ex.Message);
            }

            return users;
        }


        public User Login(string email, string password)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter("@mail", email);
                SqlParameter parm2 = new SqlParameter("@pas", password);
             
                var u =DataAccessSQL.ExecuteStoredProcedure<User>("Login", [parm1, parm2]);
                if (u.Count() != 0)
                {
                    return (User)u.ToList()[0];
                }
                else
                    return null;

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
                var u = DataAccessSQL.ExecuteStoredProcedure<User>("PostUser", listParm);
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
                var u = DataAccessSQL.ExecuteStoredProcedure<User>("PutUser", listParm);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error putting user", ex);
            }
        }

        public User GetUserByMail(string email)
        {
            try
            {
                // יצירת פרמטר לפונקציה
                NpgsqlParameter parm1 = new NpgsqlParameter("@p0", email);

                // קריאת הפונקציה ב-PostgreSQL עם הפרמטר
                var users = DataAccessPostgreSQL.ExecuteFunction<User>("GetUserByMail", new List<NpgsqlParameter> { parm1 });

                if (users.Any())
                {
                    return users.First();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by mail", ex);
            }
        }

        public bool ResetPas(string email, string password)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter("@mail", email);
                SqlParameter parm2 = new SqlParameter("@pas", password);

                var u = DataAccessSQL.ExecuteStoredProcedure<User>("ResetPassword", [parm1, parm2]);
               return true;
            }
            catch (Exception ex) { throw new Exception("Error resetting password", ex); }
        }

        //public void SaveUserRefreshToken(string email, string refreshToken)
        //{
        //    //try
        //    //{
        //    //    SqlParameter parm1 = new SqlParameter("@mail", email);
        //    //    SqlParameter parm2 = new SqlParameter("@refreshToken", refreshToken);

        //    //    await DataAccess.ExecuteStoredProcedureAsync("SaveRefreshToken", new SqlParameter[] { parm1, parm2 });
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw new Exception("Error saving refresh token", ex);
        //    //}
        //    _refreshTokenStore.SaveRefreshToken(email, refreshToken);

        //}

        //public string GetUserRefreshToken(string email)
        //{
        //    //try
        //    //{
        //    //    SqlParameter parm1 = new SqlParameter("@mail", email);

        //    //    var result = await DataAccess.ExecuteStoredProcedureAsync<string>("GetRefreshToken", new SqlParameter[] { parm1 });
        //    //    return result.FirstOrDefault();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw new Exception("Error getting refresh token", ex);
        //    //}
        //    return _refreshTokenStore.GetRefreshToken(email);

        //}
    }
}


