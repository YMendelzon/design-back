using DesigneryCommon.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

        public static List<T> ExecuteStoredProcedureWithCursor<T>(string storedProcedureName, List<NpgsqlParameter> parameters = null) where T : new()
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

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        command.ExecuteNonQuery();

                        string refCursorName = (string)refCursorParam.Value;

                        using (var fetchCommand = new NpgsqlCommand($"FETCH ALL IN \"{refCursorName}\";", connection, transaction))
                        {
                            using (var reader = fetchCommand.ExecuteReader())
                            {
                                result = PostgreSQLDataMapper.MapToList<T>(reader);
                            }
                        }
                    }

                    transaction.Commit();
                }
            }

            return result;
        }

        public static void ExecuteStoredProcedure(string storedProcedureName, List<NpgsqlParameter> parameters)
        {
            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public static T ExecuteStoredProcedureWithOutput<T>(string storedProcedureName, List<NpgsqlParameter> parameters) where T : new()
        {
            T result = new T();

            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    command.ExecuteNonQuery();

                    var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

                    foreach (var param in parameters)
                    {
                        var property = properties.FirstOrDefault(p => p.Name.Equals(param.ParameterName, StringComparison.OrdinalIgnoreCase));
                        if (property != null && param.Direction == ParameterDirection.Output && param.Value != DBNull.Value)
                        {
                            property.SetValue(result, param.Value);
                        }
                    }
                }
            }

            return result;
        }
    }
}
