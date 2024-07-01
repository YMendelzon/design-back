using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Services
{
    public class OrdersServices : IOrderService
    {


        //public IEnumerable<Order> GetHistoryOrdersByUserId(int userId)
        //{
        //    {
        //        try
        //        {
        //            SqlParameter u = new SqlParameter("@id", userId);
        //            var q = DataAccess.ExecuteStoredProcedure<Order>("GetUserHistory", u);
        //            return q.ToList();
        //        }
        //        catch
        //        {
        //            throw new Exception();
        //        };
        //    }
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


        public bool PutOrder(int id, string status)
        {
            try
            {
                // יצירת הפרמטר עבור stored procedure
                List<SqlParameter> parameters = new List<SqlParameter>() {
            new SqlParameter("@id", id),
            new SqlParameter("@status", status)
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
