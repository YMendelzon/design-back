using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;

namespace DesigneryCore.Services
{
    public class CommonQuestionsService : ICommonQuestionsService
    {
        public List<CommonQuestions> GetAllQuestions(int langId)
        {
            try { 
                SqlParameter param = new SqlParameter();

            // הגדרת השם של המשתנה הפרמטרי
            param.ParameterName = "@Lang";

            // הגדרת סוג הנתונים של המשתנה (לדוגמה, SqlDbType.Int)
            param.SqlDbType = SqlDbType.Int;

            // הגדרת ערך המשתנה
            param.Value = langId;
            
                var q = DataAccess.ExecuteStoredProcedure<CommonQuestions>("GetAllCommonQuestions", [param] );
                return q.ToList();
            }
            catch
            {
                throw new Exception("Error");
            };
        }


        public bool ChangeRating(int cqId, int rating)
        {
            try
            {
                SqlParameter parm1 = new("@id", cqId);

                SqlParameter parm2 = new("@Rating", rating);
                    
               
                var r = DataAccess.ExecuteStoredProcedure<CommonQuestions>("PutCommonQuestions",[
                    parm1, parm2]);
                return true;
            }
            catch { return false; }
        }

    }
}

