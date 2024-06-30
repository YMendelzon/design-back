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
        public List<CommonQuestions> GetAllQuestions()
        {
            try {
                var q = DataAccess.ExecuteStoredProcedure<CommonQuestions>("GetAllCommonQuestions",null);
                return q.ToList();
            }
            catch
            {
                throw new Exception();
            };
        }
        public bool PutCommonQuestions(int cqId, CommonQuestions c)
        {
            try
            {
                List<SqlParameter> listParm = new List<SqlParameter>()
                {
                 new SqlParameter("@id", cqId),
                 new SqlParameter("@questionHe", c.QuestionHe),
                 new SqlParameter("@AnswerHe", c.AnswerHe),
                 new SqlParameter("@questionEn", c.QuestionEn),
                 new SqlParameter("@AnswerEn", c.AnswerEn),
                 new SqlParameter("@Rating", c.Rating)
            };
                var r = DataAccess.ExecuteStoredProcedure<CommonQuestions>("PostCommonQuestions", listParm);
                return true;
            }
            catch { throw new Exception(); }
        }

        public bool PostCommonQuestions(CommonQuestions c)
        {
            try
            {
                List<SqlParameter> listParm = new List<SqlParameter>()
                    {
                     new SqlParameter("@questionHe", c.QuestionHe),
                     new SqlParameter("@AnswerHe", c.AnswerHe),
                     new SqlParameter("@questionEn", c.QuestionEn),
                     new SqlParameter("@AnswerEn", c.AnswerEn),
                     new SqlParameter("@Rating", c.Rating)
                };

                var result = DataAccess.ExecuteStoredProcedure<CommonQuestions>("PostCommonQuestions", listParm) ;
                return true;
            }
            catch {throw new Exception();}
        }
    }


    //SqlParameter param = new SqlParameter();

    //// הגדרת השם של המשתנה הפרמטרי
    //param.ParameterName = "@Lang";

    //// הגדרת סוג הנתונים של המשתנה (לדוגמה, SqlDbType.Int)
    //param.SqlDbType = SqlDbType.Int;

    //// הגדרת ערך המשתנה
    //param.Value = langId;
}

