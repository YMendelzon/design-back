using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DesigneryDAL

{
    public class DataAccessSQL : IDataAccess
    { 
        // משתנה לאחסון מחרוזת החיבור לשרת SQL.
        private static string _connection;

        // ממשק התצורה לקריאת הגדרות האפליקציה.
        public static IConfiguration? _config { get; set; }

        // בנאי סטטי לאתחול התצורה ומחרוזת החיבור.
        static DataAccessSQL()
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
        public static IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName,  List<SqlParameter> parameters) where T : new()
        {
            // רשימה לאחסון התוצאות מהפרוצדורה המאוחסנת.
            List<T> result = new List<T>();

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
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    // פתיחת החיבור לבסיס הנתונים.
                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        result = SQLDataMapper.MapToList<T>(dr);
                    }
                }

            }
            // החזרת רשימת התוצאות מהפרוצדורה המאוחסנת.
            return result;
        }
    }
}
