using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryDAL
{
    public interface IDataMapper
    {
        static List<T> MapToList<T>(IDataReader dr) => throw new NotImplementedException();
        static bool HasColumn(IDataReader dr, string columnName) => throw new NotImplementedException();
    }
}
