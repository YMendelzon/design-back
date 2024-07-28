using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryDAL
{
    public class DataAccessPostgreSQL : IDataAccess
    {
        private static string _connection;
        public static IConfiguration? _config { get; set; }

        static DataAccessPostgreSQL()
        {
            _config = Configuration.ReadConfigValue();
            _connection = _config["ConnectionStrings:PostgreSqlConnection"];
        }

        /// <summary>
        ///  מבצע פעולת פרוצדורת שמור ומחזיר אוסף של אובייקטים מסוג T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName">שם הפרוצדורה המאוחסנת שיש להריץ.</param>
        /// <param name="parameters"> אובייקטי SqlParameter אופציונליים המכילים פרמטרים לפרוצדורה המאוחסנת.</param>
        /// <returns>אוסף של אובייקטים מסוג T שמוחזרים על ידי הפרוצדורה המאוחסנת.</returns>
        public static List<T> ExecuteStoredProcedure<T>(string storedProcedureName, List<NpgsqlParameter> parameters) where T : new()
        {
            List<T> result = new List<T>();

            using (var connection = new NpgsqlConnection(_connection))
            {
                using (var command = new NpgsqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add output parameter for REFCURSOR
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Map your object properties here based on the data reader
                            // Example: T obj = new T { Property = reader["ColumnName"] };
                            T obj = new T();
                            result.Add(obj);
                        }
                    }
                }
            }

            return result;
        }
    }
}
