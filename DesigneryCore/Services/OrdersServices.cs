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
    public class OrdersServices : IOrderService
    {

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

        public List<Order> GetOrdersWithProductsByUserId (int userId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>() {
                    new SqlParameter("@UserId", userId)
                };
                
                var q = DataAccess.ExecuteStoredProcedure<Order>("GetOrdersWithProductsByUserId", parameters);
                return q.ToList();
            }
            catch
            {
                throw new Exception();
            }
        }

        //public bool PutOrder(int id, string status)
        //{
        //    try
        //    {
        //        // יצירת הפרמטר עבור stored procedure
        //        List<SqlParameter> parameters = new List<SqlParameter>() {
        //       new SqlParameter("@status", status),
        //    };
        //        //send to the function the param
        //        var t = DataAccess.ExecuteStoredProcedure<Order>("PutOrder", parameters);
        //        return true;
        //    }


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
    }
}
