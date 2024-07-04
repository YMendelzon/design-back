using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
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

        public List<Order> GetOrdById(int userId)
        {
            throw new NotImplementedException();
        }


        public bool PutOrder(PutOrderObject orderObject)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new() {
                new SqlParameter("@OrderID", orderObject.Id),
                new SqlParameter("@Status", orderObject.status)
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

        public bool PostOrder(Order o)
        {
            try
            {
                List<SqlParameter> listParams = new List<SqlParameter>()
                    {
                     new SqlParameter("@questionHe", o.UserID),
                     new SqlParameter("@AnswerHe", o.TotalAmount),
                     new SqlParameter("@questionEn", o.Status),
                };

                var result = DataAccess.ExecuteStoredProcedure<OrderItem>("PostOrder", listParams);
                return true;
            }
            catch { throw new Exception(); }
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

        public List<Order> GetOrderByOrderId(int orderId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                     new SqlParameter("@OrdId", orderId),
                };

                var result = DataAccess.ExecuteStoredProcedure<Order>("GetOrderByOrderId", param);
                return result.ToList();
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
