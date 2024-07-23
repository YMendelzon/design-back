using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Services
{
    public class OrdersService : IOrderService
    {
        //IOrderItemService _orderItemService;

        //public OrdersService(IOrderItemService orderItemService)
        //{
        //    _orderItemService = orderItemService;
        //}

        public List<Order> GetAllOrders()
        {
            try
            {
                var q = DataAccess.ExecuteStoredProcedure<Order>("GetAllOrders", null);
                return q.ToList();
            }
            catch
            {
                throw new Exception();
            }
        }

        public bool PutOrder(PutOrderObject orderObject)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new() {
                new SqlParameter("@OrderID", orderObject.Id),
                new SqlParameter("@Status", orderObject.Status)
                };

                // שליחה של הפרמטרים לפונקציה
                var t = DataAccess.ExecuteStoredProcedure<Order>("PutOrder", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }
        public bool PutAllPropOfOrder(int ordId, Order order)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new() {
                new SqlParameter("@OrderID", ordId),
                new SqlParameter("@Status", order.Status),
                 new SqlParameter("@TotalAmount", order.TotalAmount),
                new SqlParameter("@UserID", order.UserID)
                };

                // שליחה של הפרמטרים לפונקציה
                var t = DataAccess.ExecuteStoredProcedure<Order>("PutAllOrder", parameters);
                return true;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("");
            }
        }

        public int PostOrder(Order o)
        {
            try
            {
                List<SqlParameter> listParams = new List<SqlParameter>()
                    {
                     new SqlParameter("@UserID", o.UserID),
                     new SqlParameter("@TotalAmount", o.TotalAmount),
                     new SqlParameter("@Status", o.Status),
                     new SqlParameter("@Comment", o.Comment)
                };

                var result = DataAccess.ExecuteStoredProcedure<Order>("PostOrder", listParams);
                return result.FirstOrDefault().OrderID;
            }
            catch (Exception e) { Console.WriteLine(e); return -1; }
        }


        public List<Order> GetOrderByUserId(int userId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                     new SqlParameter("@UserId", userId),
                };

                var result = DataAccess.ExecuteStoredProcedure<Order>("GetOrderByUserId", param);
                return result.ToList();
            }
            catch { throw new Exception(); }
        }

        public Order GetOrderByOrderId(int orderId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                     new SqlParameter("@OrdId", orderId),
                };

                var result = DataAccess.ExecuteStoredProcedure<Order>("GetOrderByOrderId", param);
                return result.FirstOrDefault();
            }
            catch { throw new Exception(); }
        }

        //public bool PostOrdersItemToOrder(List<OrderItem> listOI, int orderId)
        //{
        //    try
        //    {
        //        for (int i = 0; i < listOI.Count; i++)
        //        {
        //            lis
        //            _orderItemService.PostOrderItem()
        //        }
        //    }
        //    catch { throw new Exception(); }
        //}


    }
}
