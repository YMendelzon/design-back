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
                List<NpgsqlParameter> listParams = new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("p_orderid", o.OrderID),
            new NpgsqlParameter("p_productid", o.ProductID),
            new NpgsqlParameter("p_Wording", o.Wording),
            new NpgsqlParameter("P_Comment", o.Comment),
            new NpgsqlParameter("@p_price", o.Price)
        };

                var result = DataAccessPostgreSQL.ExecuteFunction("postorderitems", listParams);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while posting the order item.", ex);
            }
        }

    }
}
