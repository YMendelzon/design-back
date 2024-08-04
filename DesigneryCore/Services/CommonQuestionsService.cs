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
using Npgsql;

namespace DesigneryCore.Services
{
    public class CommonQuestionsService : ICommonQuestionsService
    {
        public List<CommonQuestions> GetAllQuestions()
        {
            try
            {
                var questions = DataAccessPostgreSQL.ExecuteFunction<CommonQuestions>("GetAllCommonQuestions");
                return questions.ToList();
            }
            catch (Exception ex)
            {
                // אפשר להוסיף פרטים נוספים על השגיאה לצורך דיבוג
                throw new Exception("Error retrieving common questions: " + ex.Message);
            }
        }

        public bool PutCommonQuestions(int cqId, CommonQuestions c)
        {
            try
            {
                List<NpgsqlParameter> listParm = new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("@p_id", cqId),
            new NpgsqlParameter("@p_questionhe", c.QuestionHe),
            new NpgsqlParameter("@p_answerhe", c.AnswerHe),
            new NpgsqlParameter("@p_questionen", c.QuestionEn),
            new NpgsqlParameter("@p_answeren", c.AnswerEn),
            new NpgsqlParameter("@p_rating", c.Rating)
        };

                var result = DataAccessPostgreSQL.ExecuteFunction("PutCommonQuestions", listParm);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the common question.", ex);
            }
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

                var result = DataAccessSQL.ExecuteStoredProcedure<CommonQuestions>("PostCommonQuestions", listParm) ;
                return true;
            }
            catch {throw new Exception();}
        }

        public bool DeleteCommonQuestion(int cqId)
        {
            try
            {
                List<SqlParameter> listParm = new List<SqlParameter>()
                    {
                     new SqlParameter("@questionId", cqId)
                    
                };
                var result = DataAccessSQL.ExecuteStoredProcedure<CommonQuestions>("DeleteCommonQuestions", listParm);
                return true;
            }
            catch { throw new Exception(); }
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

