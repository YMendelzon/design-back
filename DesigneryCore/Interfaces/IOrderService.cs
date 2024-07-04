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

        public List<Order> GetOrderByUserId(int userId);
        public List<Order> GetOrderByOrderId (int orderId);
        public bool PutOrder(PutOrderObject orderObject);
        public List<Order> GetAllOrders();
        public bool PostOrder(Order o);

    }
}
