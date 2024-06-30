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
                throw new Exception("Error");
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
                var r = DataAccess.ExecuteStoredProcedure<CommonQuestions>("PostCommonQuestions", null);
                return true;
            }
            catch { return false; }
        }

        public bool PostCommonQuestions(CommonQuestions c)
        {
            try
            {
                //    List<SqlParameter> listParm = new List<SqlParameter>()
                //    {
                //     new SqlParameter("@questionHe", c.QuestionHe),
                //     new SqlParameter("@AnswerHe", c.AnswerHe),
                //     new SqlParameter("@questionEn", c.QuestionEn),
                //     new SqlParameter("@AnswerEn", c.AnswerEn),
                //     new SqlParameter("@Rating", c.Rating)
                //};
                SqlParameter l1 = new SqlParameter("@questionHe", c.QuestionHe);
                SqlParameter l2 = new SqlParameter("@AnswerHe", c.AnswerHe);
                SqlParameter l12 = new SqlParameter("@questionEn", c.QuestionEn);
                SqlParameter l13 = new SqlParameter("@AnswerEn", c.AnswerEn);
                SqlParameter l14 = new SqlParameter("@Rating", c.Rating);
                var result = DataAccess.ExecuteStoredProcedure<CommonQuestions>("PostCommonQuestions", [l1, l2, l12, l13, l14]) ;
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

