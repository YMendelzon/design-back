using DesigneryCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Interfaces
{
    public interface IOrderService
    {

        public List<Order> GetOrdersWithProductsByUserId(int userId);
        //bool PutOrder(int Id, string status);

        //IEnumerable<Order> GetHistoryOrdersByUserId(int userId);

        public bool PutOrder(PutOrderObject orderObject);

        public List<Order> GetAllOrders();
        
    }
}
