using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using MailKit.Search;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Services
{
    public class OrderItemService : IOrderItemService
    {
        public List<OrderItem> GetAllOrderItems()
        {
            try
            {
                var q = DataAccessPostgreSQL.ExecuteStoredProcedureWithCursor<OrderItem>("GetAllOrderItems",null);
                //var q = DataAccessSQL.ExecuteStoredProcedure<OrderItem>("GetAllOrderItems", null);
                return q;
            }
            catch
            {
                throw new Exception();
            };
        }
        public List<OrderItem> GetOrderItemByOrdId(int ordId)
        {
            try
            {
                var parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("p_ordid", NpgsqlTypes.NpgsqlDbType.Integer) { Value = ordId }
                };

                return DataAccessPostgreSQL.ExecuteFunction<OrderItem>("GetOrderItemByOrdId", parameters);
            }
            catch (Exception ex)
            {
                // Log detailed error message
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving order items.", ex);
            }
        }


        public bool PostOrderItem(OrderItem o)
        {
            try
            {
                List<SqlParameter> listParams = new List<SqlParameter>()
                    {
                     new SqlParameter("@OrderID", o.OrderID),
                     new SqlParameter("@ProductID", o.ProductID),
                     new SqlParameter("@Price", o.Price),
                     new SqlParameter("@Wording", o.Wording),
                     new SqlParameter("@Comment", o.Comment)
                };

                var result = DataAccessSQL.ExecuteStoredProcedure<OrderItem>("PostOrderItems", listParams);
                return true;
            }
            catch { throw new Exception(); }
        }
    }
}
