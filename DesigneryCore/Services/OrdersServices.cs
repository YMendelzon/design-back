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


        public IEnumerable<Order> GetHistoryOrdersByUserId(int userId)
        {
            {
                try
                {
                    SqlParameter u = new SqlParameter("@id", userId);
                    var q = DataAccess.ExecuteStoredProcedure<Order>("GetUserHistory", u);
                    return q.ToList();
                }
                catch
                {
                    throw new Exception();
                };
            }
        }
    }
}
