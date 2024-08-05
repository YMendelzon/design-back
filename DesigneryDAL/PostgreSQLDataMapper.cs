using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryDAL
{
    public class PostgreSQLDataMapper : IDataMapper
    {
        public static List<T> MapToList<T>(NpgsqlDataReader dr) where T : new()
        {
            List<T> list = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            while (dr.Read())
            {
                T obj = new T();
                foreach (var property in properties)
                {
                    if (HasColumn(dr, property.Name) && dr[property.Name] != DBNull.Value)
                    {
                        property.SetValue(obj, dr[property.Name]);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private static void MapReaderToObj<T>(NpgsqlDataReader reader, T obj) where T : new()
        {
            var properties = typeof(T).GetProperties();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var property = properties.FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));

                if (property != null && !reader.IsDBNull(i))
                {
                    property.SetValue(obj, reader.GetValue(i));
                }
            }
        }

        private static bool HasColumn(NpgsqlDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
