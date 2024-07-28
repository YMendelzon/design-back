using DesigneryCommon.Models;
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
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new NpgsqlCommand(storedProcedureName, connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var refCursorParam = new NpgsqlParameter("cur", NpgsqlTypes.NpgsqlDbType.Refcursor);
                        refCursorParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(refCursorParam);

                        // Add other parameters if any
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the REFCURSOR value
                        string refCursorName = (string)refCursorParam.Value;

                        // Fetch the data from the cursor
                        using (var fetchCommand = new NpgsqlCommand($"FETCH ALL IN \"{refCursorName}\";", connection, transaction))
                        {
                            using (var reader = fetchCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    T obj = new T();
                                    // Map your object properties here based on the data reader
                                    // Example: obj.Property = reader["ColumnName"];
                                     MapReaderToObj(reader, obj);
                                    result.Add(obj);
                                }
                            }
                        }
                    }

                    transaction.Commit();
                }
            }

            return result;
        }

        private static void MapReaderToObj<T>(NpgsqlDataReader reader, T obj) where T : new()
        {
            var properties = typeof(T).GetProperties();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var property = properties.FirstOrDefault(p => p.Name == columnName);

                if (property != null && !reader.IsDBNull(i))
                {
                    property.SetValue(obj, reader.GetValue(i));
                }
            }
        }
    }
}
