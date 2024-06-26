using DesigneryCommon.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryDAL

{
    public class DataAccess2//<T> where T : new()
    {
        // משתנה לאחסון מחרוזת החיבור לשרת SQL.
        private static string _connection;

        // ממשק התצורה לקריאת הגדרות האפליקציה.
        public static IConfiguration? _config { get; set; }

        // בנאי סטטי לאתחול התצורה ומחרוזת החיבור.
        static DataAccess2()
        {
            // אתחול התצורה על ידי קריאה להגדרות האפליקציה.
            _config = Configuration.ReadConfigValue();
            _connection = _config["ConnectionStrings:DefaultConnection"];
        }

        // מבצע פעולת פרוצדורת שמור ומחזיר אוסף של אובייקטים מסוג T.
        // פרמטרים:
        //   storedProcedureName - שם הפרוצדורה המאוחסנת שיש להריץ.
        //   parameters - אובייקטי SqlParameter אופציונליים המכילים פרמטרים לפרוצדורה המאוחסנת.
        // החזרה:
        //   אוסף של אובייקטים מסוג T שמוחזרים על ידי הפרוצדורה המאוחסנת.
        public static T ExecuteStoredProcedure<T>(string storedProcedureName, params SqlParameter[] parameters) where T : new()
        {
            // רשימה לאחסון התוצאות מהפרוצדורה המאוחסנת.
            //List<T> result = new List<T>();// = new IEnumerable<List<T>>();

            // בניית חיבור לשרת SQL.
            using (var connection = new SqlConnection(_connection))
            {
                // יצירת פקודה לביצוע הפרוצדורה המאוחסנת על החיבור הנתון.
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    // קביעת סוג הפקודה כפרוצדורה מאוחסנת.
                    command.CommandType = CommandType.StoredProcedure;

                    // הוספת פרמטרים לפקודה במידה והם קיימים.
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    // פתיחת החיבור לבסיס הנתונים.
                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        var result = DataMapper.MapToList<T>(dr);
                        return result.FirstOrDefault();
                    }
                }

            }
            // החזרת רשימת התוצאות מהפרוצדורה המאוחסנת.
            return default(T);
        }
    }
}
