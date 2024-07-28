using Microsoft.Extensions.Configuration;
using System.Data;

namespace DesigneryDAL
{
    internal interface IDataAccess
    {
        static List<T> ExecuteStoredProcedure<T>(string storedProcedureName, List<IDbDataParameter> parameters) =>  throw new NotImplementedException();
        static string _connection { get; set; }
        static IConfiguration? _config { get; set; }
    }
}
