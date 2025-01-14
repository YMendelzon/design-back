﻿using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using MailKit.Search;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Services
{
    public class OrdersService : IOrderService
    {

        public List<Order> GetAllOrders()
        {
            try
            {
                return DataAccessPostgreSQL.ExecuteFunction<Order>("GetAllOrdersF");
            }
            catch (Exception ex)
            {
                // אם תרצה לנהל שגיאות, הוסף טיפול שגיאות מתאים
                throw new Exception("An error occurred while retrieving orders.", ex);
            }
        }


        public bool PutOrder(PutOrderObject orderObject)
        {
            try
            {
                // יצירת הפרמטרים עבור הפונקציה
                List<NpgsqlParameter> parameters = new()
        {
            new NpgsqlParameter("p_orderid", orderObject.Id),
            new NpgsqlParameter("p_status", orderObject.Status)
        };

                // קריאה לפונקציה ב-PostgreSQL
                bool result = DataAccessPostgreSQL.ExecuteFunction("PutOrder", parameters);
                return result;
            }
            catch (Exception ex)
            {
                //write to logger
                throw new Exception("Error putting order", ex);
            }
        }
        //לא מעודכן בDB
        //public int PostOrder(Order o)
        //{
        //    try
        //    {
        //        List<NpgsqlParameter> listParams = new ()
        //            {
        //             new ("@UserID", o.UserID),
        //             new ("@TotalAmount", o.TotalAmount),
        //             new ("@Status", o.Status),
        //             new ("@Comment", o.Comment)
        //        };

        //        var result = DataAccessPostgreSQL.ExecuteFunction<Order>("PostOrder", listParams);
        //        return result.FirstOrDefault().OrderID;
        //    }
        //    catch (Exception e) { Console.WriteLine(e); return -1; }
        //}

        public int PostOrder(Order o)
        {
            try
            {
                var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("@p0", NpgsqlTypes.NpgsqlDbType.Integer) { Value = o.UserID },
            new NpgsqlParameter("@p1", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = o.TotalAmount },
            new NpgsqlParameter("@p2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = o.Status },
            new NpgsqlParameter("@p3", NpgsqlTypes.NpgsqlDbType.Text) { Value = o.Comment }
        };
                return DataAccessPostgreSQL.ExecuteScalar<int>("post_order", parameters);
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public List<Order> GetOrderByUserId(int userId)
        {
            try
            {
                var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("user_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = userId }
        };

                return DataAccessPostgreSQL.ExecuteFunction<Order>("GetOrderByUserId", parameters);
            }
            catch (Exception ex)
            {
                // Log detailed error message
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving orders and users.", ex);
            }
        }




        public Order GetOrderByOrderId(int orderId)
        {
            //    try
            //    {
            //        List<SqlParameter> param = new List<SqlParameter>()
            //        {
            //             new SqlParameter("@orderid", orderId),
            //             new SqlParameter("@userid", orderId),
            //             new SqlParameter("@totalamount", orderId),
            //             new SqlParameter("@status", orderId)
            //        };

            //        var result = DataAccessPostgreSQL.ExecuteStoredProcedure<Order>("GetOrderByOrderId", param);
            //       // var result = DataAccessSQL.ExecuteStoredProcedure<Order>("GetOrderByOrderId", param);
            //        return result.FirstOrDefault();
            //    }
            //    catch { throw new Exception(); }
            //}

            try
            {
                List<NpgsqlParameter> param = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("p_ordid", NpgsqlTypes.NpgsqlDbType.Integer) { Value = orderId },
                    new NpgsqlParameter("orderid", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output },
                    new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output },
                    new NpgsqlParameter("totalamount", NpgsqlTypes.NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output },
                    new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Output }
                };

                var res = DataAccessPostgreSQL.ExecuteStoredProcedureWithOutput<Order>("GetOrderByOrderId", param);

                var result = new Order
                {
                    OrderID = (int)param[1].Value,
                    UserID = (int)param[2].Value,
                    TotalAmount = (decimal)param[3].Value,
                    Status = param[4].Value.ToString()
                };

                return result;
            }
            catch
            {
                throw new Exception();
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
}