using DesigneryCommon.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryDAL

{
    public class DataAccess<T> where T : new()
    {
        // משתנה לאחסון מחרוזת החיבור לשרת SQL.
        private static string _connection;

        // ממשק התצורה לקריאת הגדרות האפליקציה.
        public static IConfiguration? _config { get; set; }

        // בנאי סטטי לאתחול התצורה ומחרוזת החיבור.
        static DataAccess()
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
        public static IEnumerable<T> ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
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
                        command.Parameters.AddRange(parameters);
                    }

                    // פתיחת החיבור לבסיס הנתונים.
                    connection.Open();
                    // ביצוע הפקודה וקריאת התוצאות לאובייקטים מסוג T.
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            // יצירת אינסטנסיה של אובייקט מסוג T לאחסון כל תוצאה מהקורא.
                            //T item = Activator.CreateInstance<T>();
                            // TODO: עיבוד העמודות של הקורא למאפייני האובייקט כאן אם נדרש.
                            //יצירת משתנה חדש מסוג המחלקה המדוברת
                            T item = new T();

                            // מעבר על כל המשתנים שלו
                            foreach (var prop in typeof(T).GetProperties())
                            {
                                // שולף אותם מההמרה
                                if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                                {
                                    //itemסוג של מעדכן את ה
                                    prop.SetValue(item, reader[prop.Name]);
                                }
                            }
                            // הוספת האובייקט לרשימת התוצאות.
                            result.Add(item);
                        }
                    }
                }
            }

            // החזרת רשימת התוצאות מהפרוצדורה המאוחסנת.
            return result;
        }
    }
}
