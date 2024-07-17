using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using MailKit.Search;
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
                var q = DataAccess.ExecuteStoredProcedure<OrderItem>("GetAllOrderItems", null);
                return q.ToList();
            }
            catch
            {
                throw new Exception();
            };
        }
        public List<OrderItem> GetOrderItemByOrdId(int orderId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@OrdId", orderId)
                };
                var r = DataAccess.ExecuteStoredProcedure<OrderItem>("GetOrderItemByOrdId", param);
                return r.ToList();
            }
            catch
            {
                throw new Exception();
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
                     new SqlParameter("@Quantity", o.Quantity),
                     new SqlParameter("@Price", o.Price),
                };

                var result = DataAccess.ExecuteStoredProcedure<OrderItem>("PostOrderItems", listParams);
                return true;
            }
            catch { throw new Exception(); }
        }
    }
}
