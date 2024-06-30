using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryDAL
{
    //this page i wrote myself
    //so probably i have mistakes...
    public static class DataMapper
    {

        // class to map the property from the reader
        public static List<T> MapToList<T>(SqlDataReader dr) where T : new()
        {
            //list to save the solution
            List<T> list = new List<T>();
            //declare properties for all the variable that related to the object
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            //read from the what return from the procedure code
            while (dr.Read())
            {
                // new object for each line
                T obj = new T();
                //go over all the properties
                foreach (var property in properties)
                {
                    //get the value from the property by the name
                    if (HasColumn(dr, property.Name) && dr[property.Name] != DBNull.Value)
                    {
                        //save it...
                        property.SetValue(obj, dr[property.Name]);
                    }
                }
                //and add it to the list
                list.Add(obj);
            }
            //return the list
            return list;
        }

        //some check about the right properties...
        private static bool HasColumn(SqlDataReader dr, string columnName)
        {
            //go over all the field in the table
            for (int i = 0; i < dr.FieldCount; i++)
            {
                //if the name is ????
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
